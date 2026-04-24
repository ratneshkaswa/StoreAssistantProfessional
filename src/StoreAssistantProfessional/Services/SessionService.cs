namespace StoreAssistantProfessional.Services;

public enum Role
{
    User,
    Admin
}

public interface ISessionService
{
    Role Current { get; }
    TimeSpan? UnlockLockoutRemaining { get; }
    event Action? Changed;
    void EnterAdmin();
    void ExitAdmin();
    void RecordFailedUnlock();
}

public sealed class SessionService : ISessionService
{
    public const int MaxUnlockAttempts = 5;
    public const int LockoutDurationSeconds = 30;

    private readonly object _lock = new();
    private Role _current = Role.User;
    private int _failedUnlockAttempts;
    private DateTime? _lockoutUntilUtc;

    public Role Current
    {
        get { lock (_lock) return _current; }
    }

    public TimeSpan? UnlockLockoutRemaining
    {
        get
        {
            lock (_lock)
            {
                if (_lockoutUntilUtc is null) return null;
                var remaining = _lockoutUntilUtc.Value - DateTime.UtcNow;
                if (remaining <= TimeSpan.Zero)
                {
                    _lockoutUntilUtc = null;
                    return null;
                }
                return remaining;
            }
        }
    }

    public event Action? Changed;

    public void EnterAdmin()
    {
        lock (_lock)
        {
            _failedUnlockAttempts = 0;
            _lockoutUntilUtc = null;
            if (_current == Role.Admin) return;
            _current = Role.Admin;
        }
        Changed?.Invoke();
    }

    public void ExitAdmin()
    {
        lock (_lock)
        {
            if (_current == Role.User) return;
            _current = Role.User;
        }
        Changed?.Invoke();
    }

    public void RecordFailedUnlock()
    {
        lock (_lock)
        {
            _failedUnlockAttempts++;
            if (_failedUnlockAttempts % MaxUnlockAttempts == 0)
                _lockoutUntilUtc = DateTime.UtcNow.AddSeconds(LockoutDurationSeconds);
        }
        Changed?.Invoke();
    }
}
