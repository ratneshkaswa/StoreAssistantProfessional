using MudBlazor;
using MudBlazor.Utilities;

namespace StoreAssistantProfessional.Theme;

public static class BrandTheme
{
    public static readonly MudTheme Instance = new()
    {
        PaletteLight = new PaletteLight
        {
            Primary = new MudColor("#E8B584"),            // Warm peach / dusty saffron
            PrimaryContrastText = new MudColor("#4A2F1A"),
            Secondary = new MudColor("#A8C0A3"),          // Muted sage
            SecondaryContrastText = new MudColor("#2E3E2B"),
            Tertiary = new MudColor("#C9A8C8"),           // Soft mauve
            TertiaryContrastText = new MudColor("#3E2E3C"),
            Info = new MudColor("#9BBED1"),               // Soft sky
            InfoContrastText = new MudColor("#1E3644"),
            Success = new MudColor("#8FBF87"),            // Soft green
            SuccessContrastText = new MudColor("#1E3A1C"),
            Warning = new MudColor("#E8C084"),            // Soft amber
            WarningContrastText = new MudColor("#4A3318"),
            Error = new MudColor("#D89090"),              // Muted rose
            ErrorContrastText = new MudColor("#4A1E1E"),
            AppbarBackground = new MudColor("#E8B584"),
            AppbarText = new MudColor("#4A2F1A"),
            Background = new MudColor("#FAF6F0"),         // Cream
            Surface = new MudColor("#FFFFFF"),
            DrawerBackground = new MudColor("#FAF6F0"),
            DrawerText = new MudColor("#3A3A3A"),
            TextPrimary = new MudColor("#3A3A3A"),        // Soft charcoal, not harsh black
            TextSecondary = new MudColor("#7A7268"),      // Warm grey
            TextDisabled = new MudColor("#B5AEA6"),
            ActionDefault = new MudColor("#6B6158"),
            ActionDisabled = new MudColor("#C5BEB6"),
            ActionDisabledBackground = new MudColor("#F0EBE3"),
            LinesDefault = new MudColor("#00000014"),     // 8% black
            LinesInputs = new MudColor("#00000026"),      // 15% black
            Divider = new MudColor("#00000014"),
            DividerLight = new MudColor("#0000000A"),
        },
        Typography = new Typography
        {
            Default = new DefaultTypography
            {
                FontFamily = ["Segoe UI", "system-ui", "sans-serif"],
            },
        },
    };
}
