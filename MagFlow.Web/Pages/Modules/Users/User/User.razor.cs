using MagFlow.BLL.Services;
using MagFlow.Shared.DTOs.CoreScope;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Enumerators;
using MudBlazor;

namespace MagFlow.Web.Pages.Modules.Users.User
{
    public partial class User : AuthComponentBase
    {
        private UserDTO? _user { get; set; }

        private SectionsEnums.UserDetailsSection _currentSection = SectionsEnums.UserDetailsSection.Profile;
        private bool _isMenuOpened = false;

        bool _loadingDelete = false;
        bool _loadingBlock = false;
        bool _loadingSave = false;
        bool _isBusy = false;

        protected override async Task OnParametersSetAsync()
        {
            if (!string.IsNullOrEmpty(UserId) && _user == null)
            {
                if (Guid.TryParse(UserId, out var uid))
                    _user = await UserService.GetUser(uid, true);
                if (_user == null)
                    UserId = string.Empty;
            }
        }

        private async Task BlockUser()
        {
            if (!HasModulePermission("Users", PermissionFlags.Edit))
                return;

            if (_user == null || _loadingBlock || _isBusy)
                return;

            try
            {
                _isBusy = true;
                _loadingBlock = true;

                await Task.Delay(2000);
            }
            finally
            {
                _isBusy = false;
                _loadingBlock = false;
            }
        }

        private async Task DeleteUser()
        {
            if (!HasModulePermission("Users", PermissionFlags.Delete))
                return;

            if (_user == null || _loadingDelete || _isBusy)
                return;

            try
            {
                _isBusy = true;
                _loadingDelete = true;

                await Task.Delay(2000);
            }
            finally
            {
                _isBusy = false;
                _loadingDelete = false;
            }
        }

        private async Task SaveChanges()
        {
            if (!HasModulePermission("Users", PermissionFlags.Edit))
                return;

            if (_user == null || _loadingSave || _isBusy)
                return;

            try
            {
                _isBusy = true;
                _loadingSave = true;

                await Task.Delay(2000);
            }
            finally
            {
                _isBusy = false;
                _loadingSave = false;
            }
        }

        private void OnSectionChanged(SectionsEnums.UserDetailsSection section)
        {
            if (_currentSection == section)
                return;
            _currentSection = section;
            StateHasChanged();
        }

        private string GetNavClass(SectionsEnums.UserDetailsSection section)
        {
            return _currentSection == section
                ? "mud-primary-text mud-nav-link-active"
                : "";
        }

        private Color GetIconColor(SectionsEnums.UserDetailsSection section)
        {
            return _currentSection == section
                ? Color.Primary
                : Color.Dark;
        }

    }
}
