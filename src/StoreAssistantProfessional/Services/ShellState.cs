namespace StoreAssistantProfessional.Services;

public enum CashMode { SingleDrawer, PettyBox }

public interface IShellState
{
    event Action? Changed;

    bool TrainingMode { get; }
    bool PrivacyMode { get; }
    CashMode CashMode { get; }

    void ToggleTraining();
    void TogglePrivacy();
    void SetCashMode(CashMode mode);
}

public sealed class ShellState : IShellState
{
    private readonly object _lock = new();
    private bool _training;
    private bool _privacy;
    private CashMode _cashMode = CashMode.SingleDrawer;

    public event Action? Changed;

    public bool TrainingMode { get { lock (_lock) return _training; } }
    public bool PrivacyMode  { get { lock (_lock) return _privacy; } }
    public CashMode CashMode { get { lock (_lock) return _cashMode; } }

    public void ToggleTraining()
    {
        lock (_lock) _training = !_training;
        Changed?.Invoke();
    }

    public void TogglePrivacy()
    {
        lock (_lock) _privacy = !_privacy;
        Changed?.Invoke();
    }

    public void SetCashMode(CashMode mode)
    {
        lock (_lock) _cashMode = mode;
        Changed?.Invoke();
    }
}
