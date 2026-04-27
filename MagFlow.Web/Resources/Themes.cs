using MudBlazor;

namespace MagFlow.Web.Resources
{
    public static class Themes
    {
        public static readonly MudTheme MAIN = new MudTheme()
        {
            PaletteLight = new PaletteLight
            {
                Primary = "#0D6EFD",
                Secondary = "#E0E0E0",
                SecondaryContrastText = "#2E2E2E",
                TextSecondary = "#2E2E2E",
                Background = "#FFFFFF",
                AppbarBackground = "#FFFFFF",
                TextPrimary = "#272C34",
                //TextSecondary = "#495057",
                //ActionDefault = "#BDBDBD",
                ActionDefault = "#757575",
            },
            Typography = new Typography
            {
                Button = new ButtonTypography
                {
                    TextTransform = "none"
                },
                Default = new DefaultTypography()
                {
                    FontFamily = new []
                    {
                        "Roboto"
                    }
                },
            }
        };
    }
}
