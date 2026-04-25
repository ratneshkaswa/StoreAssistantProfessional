using Microsoft.EntityFrameworkCore;
using StoreAssistantProfessional.Data;
using StoreAssistantProfessional.Models;

namespace StoreAssistantProfessional.Services;

public interface IActivityService : IDisposable
{
    event Action? IdleChanged;
    event Action? AutoDropped;

    DateTime LastActivityAt { get; }
    bool IsIdle { get; }
    TimeSpan IdleFor { get; }

    void RegisterActivity();
    Task LogEventAsync(string kind, string? role = null, string? reason = null, string? detail = null);
}

public sealed class ActivityService : IActivityService
{
    private readonly ISessionService _session;
    private readonly IAppSettingsService _settings;
    private readonly IDbContextFactory<AppDbContext> _dbFactory;
    private readonly Timer _timer;
    private readonly object _lock = new();
    private DateTime _lastActivity = DateTime.UtcNow;
    private bool _isIdle;
    private int _adminDropMinutes = 5;
    private int _idleScreenMinutes = 10;

    public event Action? IdleChanged;
    public event Action? AutoDropped;

    public ActivityService(
        ISessionService session,
        IAppSettingsService settings,
        IDbContextFactory<AppDbContext> dbFactory)
    {
        _session = session;
        _settings = settings;
        _dbFactory = dbFactory;
        _timer = new Timer(Tick, null, TimeSpan.FromSeconds(15), TimeSpan.FromSeconds(15));
        _ = LoadThresholdsAsync();
    }

    private async Task LoadThresholdsAsync()
    {
        try
        {
            var s = await _settings.LoadAsync();
            _adminDropMinutes = Math.Max(1, s.AdminAutoDropMinutes);
            _idleScreenMinutes = Math.Max(2, s.IdleScreenMinutes);
        }
        catch { /* DB might not be ready yet */ }
    }

    public DateTime LastActivityAt
    {
        get { lock (_lock) return _lastActivity; }
    }

    public TimeSpan IdleFor => DateTime.UtcNow - LastActivityAt;
    public bool IsIdle { get { lock (_lock) return _isIdle; } }

    public void RegisterActivity()
    {
        bool wasIdle;
        lock (_lock)
        {
            _lastActivity = DateTime.UtcNow;
            wasIdle = _isIdle;
            _isIdle = false;
        }
        if (wasIdle) IdleChanged?.Invoke();
    }

    private void Tick(object? state)
    {
        try
        {
            var idle = IdleFor;

            if (_session.Current == Role.Admin && idle >= TimeSpan.FromMinutes(_adminDropMinutes))
            {
                _session.ExitAdmin();
                _ = LogEventAsync("AutoDrop", role: "User", reason: "idle");
                AutoDropped?.Invoke();
            }

            var nextIdle = idle >= TimeSpan.FromMinutes(_idleScreenMinutes);
            bool changed;
            lock (_lock)
            {
                changed = _isIdle != nextIdle;
                _isIdle = nextIdle;
            }
            if (changed) IdleChanged?.Invoke();

            _ = LoadThresholdsAsync();
        }
        catch { /* swallow — never let timer crash */ }
    }

    public async Task LogEventAsync(string kind, string? role = null, string? reason = null, string? detail = null)
    {
        try
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            db.SessionEvents.Add(new SessionEvent
            {
                At = DateTime.UtcNow,
                Kind = kind,
                Role = role,
                Reason = reason,
                Detail = detail
            });
            await db.SaveChangesAsync();
        }
        catch { /* logging is best-effort */ }
    }

    public void Dispose() => _timer.Dispose();
}
