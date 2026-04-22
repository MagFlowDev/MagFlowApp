using MagFlow.Shared.DTOs.CoreScope;
using MagFlow.Shared.Models.Enumerators;
using MagFlow.Web.Resources;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace MagFlow.Web.Pages.Modules.Users
{
    public partial class UsersList : AuthComponentBase
    {
        MudDataGrid<UserDTO> _usersDataGrid;
        string? _searchString = null;

        bool _isBusy = false;

        private async Task<GridData<UserDTO>> ServerReloadUsers(GridState<UserDTO> state, CancellationToken token)
        {
            var sortDefinition = state.SortDefinitions.FirstOrDefault();
            string? sortBy = sortDefinition?.SortBy;
            if (Guid.TryParse(sortBy, out _))
            {
                var column = _usersDataGrid.RenderedColumns.FirstOrDefault(c => c.PropertyName == sortBy);
                sortBy = column?.Tag?.ToString();
            }
            sortBy = sortBy ?? nameof(UserDTO.LastName);
            var response = await CompanyService.GetUsers(state.Page, state.PageSize, _searchString, sortBy, sortDefinition?.Descending == true);

            return new GridData<UserDTO>
            {
                Items = response.Elements,
                TotalItems = response.TotalCount,
            };
        }

        private async Task OpenAddUserModal()
        {
            if (!HasModulePermission("Users", PermissionFlags.Add))
                return;

            var dialog = await DialogService.ShowAsync<AddUserDialog>(Localizer[Langs.AddUser]);
            var confirmation = await dialog.Result;
            if (confirmation?.Data is bool result && result == true)
                await _usersDataGrid.ReloadServerData();
        }


        private void OpenUserDetails(UserDTO user)
        {
            if (!HasModulePermission("Users", PermissionFlags.Read))
                return;

            NavigationManager.NavigateTo($"/user/{user.Id}");
        }


        private Dictionary<Guid, bool> _loadingBlock = [];
        private bool LoadingBlock(Guid id) => _loadingBlock.TryGetValue(id, out var value) && value;
        private async Task BlockUser(UserDTO user)
        {
            if (!HasModulePermission("Users", PermissionFlags.Edit))
                return;

            if (_isBusy || (_loadingBlock.TryGetValue(user.Id, out var loading) && loading))
                return;

            try
            {
                _isBusy = true;
                _loadingBlock[user.Id] = true;

                await Task.Delay(2000);
            }
            finally
            {
                _isBusy = false;
                _loadingBlock[user.Id] = false;
            }
        }

        private Dictionary<Guid, bool> _loadingDelete = [];
        private bool LoadingDelete(Guid id) => _loadingDelete.TryGetValue(id, out var value) && value;
        private async Task DeleteUser(UserDTO user)
        {
            if (!HasModulePermission("Users", PermissionFlags.Delete))
                return;

            if (_isBusy || (_loadingDelete.TryGetValue(user.Id, out var loading) && loading))
                return;

            try
            {
                _isBusy = true;
                _loadingDelete[user.Id] = true;

                await Task.Delay(2000);
            }
            finally
            {
                _isBusy = false;
                _loadingDelete[user.Id] = false;
            }
        }
    }
}
