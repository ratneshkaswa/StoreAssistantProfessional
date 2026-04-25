using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace StoreAssistantProfessional.Services;

public sealed class CounterPresence
{
    public int Pid { get; set; }
    public string MachineName { get; set; } = "";
    public string Counter { get; set; } = "";
    public DateTime HeartbeatAt { get; set; }
}

public interface ICounterPresenceService : IDisposable
{
    event Action? Changed;
    string CurrentCounter { get; }
    IReadOnlyList<CounterPresence> Others { get; }
}

public sealed class CounterPresenceService : ICounterPresenceService
{
    private readonly Timer _timer;
    private readonly string _dir;
    private readonly string _myFile;
    private readonly object _lock = new();
    private List<CounterPresence> _others = new();

    public event Action? Changed;
    public string CurrentCounter { get; }

    public CounterPresenceService()
    {
        _dir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "StoreAssistantProfessional", "presence");
        Directory.CreateDirectory(_dir);

        var pid = Environment.ProcessId;
        CurrentCounter = $"Counter-{pid % 100:D2}";
        _myFile = Path.Combine(_dir, $"{pid}.json");

        Heartbeat();
        _timer = new Timer(_ => Tick(), null, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10));
    }

    public IReadOnlyList<CounterPresence> Others { get { lock (_lock) return _others.ToList(); } }

    private void Tick()
    {
        try
        {
            Heartbeat();
            ScanOthers();
        }
        catch { /* swallow — never let timer crash */ }
    }

    private void Heartbeat()
    {
        var me = new CounterPresence
        {
            Pid = Environment.ProcessId,
            MachineName = Environment.MachineName,
            Counter = CurrentCounter,
            HeartbeatAt = DateTime.UtcNow
        };
        try { File.WriteAllText(_myFile, JsonSerializer.Serialize(me)); } catch { }
    }

    private void ScanOthers()
    {
        var found = new List<CounterPresence>();
        var stale = new List<string>();
        foreach (var file in Directory.EnumerateFiles(_dir, "*.json"))
        {
            if (file == _myFile) continue;
            try
            {
                var json = File.ReadAllText(file);
                var p = JsonSerializer.Deserialize<CounterPresence>(json);
                if (p is null) continue;
                if ((DateTime.UtcNow - p.HeartbeatAt).TotalSeconds > 60)
                {
                    stale.Add(file);
                    continue;
                }
                if (!IsAlive(p.Pid))
                {
                    stale.Add(file);
                    continue;
                }
                found.Add(p);
            }
            catch { stale.Add(file); }
        }

        foreach (var f in stale) try { File.Delete(f); } catch { }

        bool changed;
        lock (_lock)
        {
            changed = !_others.Select(o => o.Pid).OrderBy(x => x).SequenceEqual(found.Select(o => o.Pid).OrderBy(x => x));
            _others = found;
        }
        if (changed) Changed?.Invoke();
    }

    private static bool IsAlive(int pid)
    {
        try { Process.GetProcessById(pid); return true; }
        catch { return false; }
    }

    public void Dispose()
    {
        _timer.Dispose();
        try { File.Delete(_myFile); } catch { }
    }
}
