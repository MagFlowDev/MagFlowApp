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
            if (!HasModulePermission("Users", PermissionFlags.Create))
                return;

            var dialog = await DialogService.ShowAsync<AddUserDialog>(Localizer[Langs.AddUser]);
            var confirmation = await dialog.Result;
            if (confirmation?.Data is bool result && result == true)
                await _usersDataGrid.ReloadServerData();
        }


        private void OpenUserDetails(UserDTO user)
        {
            NavigationManager.NavigateTo($"/user/{user.Id}");
        }


        private async Task BlockUser(UserDTO user)
        {

        }

        private async Task DeleteUser(UserDTO user)
        {

        }
    }
}
