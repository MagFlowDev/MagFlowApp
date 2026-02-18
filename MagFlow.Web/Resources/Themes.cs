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
                Secondary = "#6C757D",
                Background = "#FFFFFF",
                AppbarBackground = "#FFFFFF",
                TextPrimary = "#272C34",
                TextSecondary = "#495057",
                ActionDefault = "#BDBDBD",
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
