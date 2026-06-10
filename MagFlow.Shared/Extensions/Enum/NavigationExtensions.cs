using MagFlow.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;
using MudBlazor;

namespace MagFlow.Shared.Extensions.Enum
{
    public static class NavigationExtensions
    {
        // NavClass
        public static string GetNavClass(this System.Enum currentSection, System.Enum section)
            => NavClass((dynamic)currentSection, (dynamic)section);

        // IconColor
        public static Color GetIconColor(this System.Enum currentSection, System.Enum section)
            => IconColor((dynamic)currentSection, (dynamic)section);


        private static string NavClass<T>(T current, T target) where T : struct, System.Enum
        {
            return EqualityComparer<T>.Default.Equals(current, target)
                ? "mud-primary-text mud-nav-link-active text-nowrap"
                : "text-nowrap";
        }

        private static Color IconColor<T>(T current, T target) where T : struct, System.Enum
        {
            return EqualityComparer<T>.Default.Equals(current, target)
                ? Color.Primary
                : Color.Dark;
        }
    }
}
