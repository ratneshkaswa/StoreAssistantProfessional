using MudBlazor;

namespace StoreAssistantProfessional.Components.Shared;

// Centralized snackbar configuration so duration / close-icon decisions live
// in one place. The four onboarding pages used to redefine ShowOk/ShowError
// each, drifting in subtle ways. Extension methods on ISnackbar keep call
// sites short (`Snackbar.ShowOk("Saved.")`) without forcing a base class.
public static class SnackbarHelpers
{
    public const int OkDurationMs = 3000;
    public const int InfoDurationMs = 5000;
    public const int ErrorDurationMs = 8000;

    public static void ShowOk(this ISnackbar s, string message) =>
        s.Add(message, Severity.Success, c => c.VisibleStateDuration = OkDurationMs);

    public static void ShowInfo(this ISnackbar s, string message) =>
        s.Add(message, Severity.Info, c => c.VisibleStateDuration = InfoDurationMs);

    public static void ShowWarning(this ISnackbar s, string message) =>
        s.Add(message, Severity.Warning, c =>
        {
            c.VisibleStateDuration = InfoDurationMs;
            c.ShowCloseIcon = true;
        });

    public static void ShowError(this ISnackbar s, string message) =>
        s.Add(message, Severity.Error, c =>
        {
            c.VisibleStateDuration = ErrorDurationMs;
            c.ShowCloseIcon = true;
        });
}
