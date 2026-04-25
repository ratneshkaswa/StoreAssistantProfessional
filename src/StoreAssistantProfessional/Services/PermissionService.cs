using StoreAssistantProfessional.Models;

namespace StoreAssistantProfessional.Services;

public enum AppAction
{
    Discount,
    Refund,
    VoidBill,
    EditSavedBill,
    AdjustStock,
    EditSettings,
    OpenDrawer,
    RunReports,
    Export,
    Reset,
    PriceAboveMrp
}

public interface IPermissionService
{
    Task<bool> CanAsync(AppAction action);
    bool Can(AppAction action, AppSettings settings, Role role);
}

public sealed class PermissionService : IPermissionService
{
    private readonly IAppSettingsService _settings;
    private readonly ISessionService _session;

    public PermissionService(IAppSettingsService settings, ISessionService session)
    {
        _settings = settings;
        _session = session;
    }

    public async Task<bool> CanAsync(AppAction action)
    {
        if (_session.Current == Role.Admin) return true;
        var s = await _settings.LoadAsync();
        return Can(action, s, _session.Current);
    }

    public bool Can(AppAction action, AppSettings s, Role role)
    {
        if (role == Role.Admin) return true;
        return action switch
        {
            AppAction.Discount       => s.UserCanGiveDiscount,
            AppAction.Refund         => s.UserCanRefund,
            AppAction.VoidBill       => s.UserCanVoidBill,
            AppAction.EditSavedBill  => s.UserCanEditSavedBill,
            AppAction.AdjustStock    => s.UserCanAdjustStock,
            AppAction.EditSettings   => s.UserCanEditSettings,
            AppAction.OpenDrawer     => s.UserCanOpenDrawer,
            AppAction.RunReports     => s.UserCanRunReports,
            AppAction.Export         => s.UserCanExport,
            AppAction.Reset          => false, // always Admin
            AppAction.PriceAboveMrp  => false, // always Admin
            _                        => false
        };
    }
}
