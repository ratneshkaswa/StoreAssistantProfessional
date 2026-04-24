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
    public Role Current { get; private set; } = Role.User;

    public event Action? Changed;

    public void EnterAdmin()
    {
        if (Current == Role.Admin) return;
        Current = Role.Admin;
        Changed?.Invoke();
    }

    public void ExitAdmin()
    {
        if (Current == Role.User) return;
        Current = Role.User;
        Changed?.Invoke();
    }
}
