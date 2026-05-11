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
        public static string GetNavClass(this SectionsEnums.UserSettingsSection currentSection, SectionsEnums.UserSettingsSection section)
            => NavClass(currentSection, section);

        public static string GetNavClass(this SectionsEnums.CompanySettingsSection currentSection, SectionsEnums.CompanySettingsSection section)
            => NavClass(currentSection, section);

        public static string GetNavClass(this SectionsEnums.UsersModuleSection currentSection, SectionsEnums.UsersModuleSection section)
            => NavClass(currentSection, section);

        public static string GetNavClass(this SectionsEnums.UserDetailsSection currentSection, SectionsEnums.UserDetailsSection section)
            => NavClass(currentSection, section);

        public static string GetNavClass(this SectionsEnums.WaresModuleSection currentSection, SectionsEnums.WaresModuleSection section)
            => NavClass(currentSection, section);


        // IconColor
        public static Color GetIconColor(this SectionsEnums.UserSettingsSection currentSection, SectionsEnums.UserSettingsSection section)
            => IconColor(currentSection, section);

        public static Color GetIconColor(this SectionsEnums.CompanySettingsSection currentSection, SectionsEnums.CompanySettingsSection section)
            => IconColor(currentSection, section);

        public static Color GetIconColor(this SectionsEnums.UsersModuleSection currentSection, SectionsEnums.UsersModuleSection section)
            => IconColor(currentSection, section);

        public static Color GetIconColor(this SectionsEnums.UserDetailsSection currentSection, SectionsEnums.UserDetailsSection section)
            => IconColor(currentSection, section);

        public static Color GetIconColor(this SectionsEnums.WaresModuleSection currentSection, SectionsEnums.WaresModuleSection section)
            => IconColor(currentSection, section);






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
