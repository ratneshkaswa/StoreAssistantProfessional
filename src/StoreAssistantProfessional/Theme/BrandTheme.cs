using MudBlazor;
using MudBlazor.Utilities;

namespace StoreAssistantProfessional.Theme;

public static class BrandTheme
{
    public const string HeroGradientLight = "#EDBE91";
    public const string HeroGradientDark  = "#DFA878";
    public const string HeroTextColor     = "#4A2F1A";
    public const string HeroDividerColor  = "rgba(74, 47, 26, 0.25)";

    public static readonly MudTheme Instance = new()
    {
        PaletteLight = CreatePalette(),
        PaletteDark = CreatePalette(),
        Typography = new Typography
        {
            Default = new DefaultTypography
            {
                FontFamily = ["Segoe UI", "system-ui", "sans-serif"],
            },
            H3 = new H3Typography
            {
                FontWeight = "500",
                LineHeight = "1.25",
            },
            H4 = new H4Typography
            {
                FontWeight = "500",
                LineHeight = "1.3",
            },
        },
    };

    private static PaletteLight CreatePalette() => new()
    {
        Primary = new MudColor("#E8B584"),
        PrimaryContrastText = new MudColor(HeroTextColor),
        Secondary = new MudColor("#A8C0A3"),
        SecondaryContrastText = new MudColor("#2E3E2B"),
        Tertiary = new MudColor("#C9A8C8"),
        TertiaryContrastText = new MudColor("#3E2E3C"),
        Info = new MudColor("#9BBED1"),
        InfoContrastText = new MudColor("#1E3644"),
        Success = new MudColor("#8FBF87"),
        SuccessContrastText = new MudColor("#1E3A1C"),
        Warning = new MudColor("#E8C084"),
        WarningContrastText = new MudColor("#4A3318"),
        Error = new MudColor("#D89090"),
        ErrorContrastText = new MudColor("#4A1E1E"),
        AppbarBackground = new MudColor("#E8B584"),
        AppbarText = new MudColor(HeroTextColor),
        Background = new MudColor("#FAF6F0"),
        Surface = new MudColor("#FFFFFF"),
        DrawerBackground = new MudColor("#FAF6F0"),
        DrawerText = new MudColor("#3A3A3A"),
        TextPrimary = new MudColor("#3A3A3A"),
        TextSecondary = new MudColor("#7A7268"),
        TextDisabled = new MudColor("#B5AEA6"),
        ActionDefault = new MudColor("#6B6158"),
        ActionDisabled = new MudColor("#C5BEB6"),
        ActionDisabledBackground = new MudColor("#F0EBE3"),
        LinesDefault = new MudColor("#00000014"),
        LinesInputs = new MudColor("#00000026"),
        Divider = new MudColor("#00000014"),
        DividerLight = new MudColor("#0000000A"),
    };
}
