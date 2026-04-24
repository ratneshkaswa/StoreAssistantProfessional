using MudBlazor;
using MudBlazor.Utilities;

namespace StoreAssistantProfessional.Theme;

public static class BrandTheme
{
    public static readonly MudTheme Instance = new()
    {
        PaletteLight = new PaletteLight
        {
            Primary = new MudColor("#E65100"),          // Deep orange — warm, retail-forward
            PrimaryContrastText = new MudColor("#FFFFFF"),
            Secondary = new MudColor("#00695C"),        // Deep teal — grounds the warmth
            SecondaryContrastText = new MudColor("#FFFFFF"),
            Tertiary = new MudColor("#6A1B9A"),         // Rich purple — premium accent
            TertiaryContrastText = new MudColor("#FFFFFF"),
            Info = new MudColor("#0277BD"),
            Success = new MudColor("#2E7D32"),
            Warning = new MudColor("#EF6C00"),
            Error = new MudColor("#C62828"),
            AppbarBackground = new MudColor("#E65100"),
            Background = new MudColor("#FFF8F3"),       // Off-white with warm tint
            Surface = new MudColor("#FFFFFF"),
            TextPrimary = new MudColor("#1C1B1F"),
            TextSecondary = new MudColor("#5A4A42"),
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
