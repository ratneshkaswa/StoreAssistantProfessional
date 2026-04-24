namespace StoreAssistantProfessional.Services;

public enum Role
{
    User,
    Admin
}

public interface ISessionService
{
    Role Current { get; }
    event Action? Changed;
    void EnterAdmin();
    void ExitAdmin();
}

public sealed class SessionService : ISessionService
{
    private readonly object _lock = new();
    private Role _current = Role.User;

    public Role Current
    {
        get { lock (_lock) return _current; }
    }

    public event Action? Changed;

    public void EnterAdmin()
    {
        lock (_lock)
        {
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
}
