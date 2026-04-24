using MagFlow.Shared.DTOs.CoreScope;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Enumerators;
using MagFlow.Web.Components.Dialogs;
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
        private bool _loadingBlockMany { get; set; }
        private bool _loadingUnblockMany { get; set; }
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

                var parameters = new DialogParameters<ConfirmBlockDialog> { { x => x.ContentText, string.Format(Localizer[Langs.BlockUserConfirmation], user.Email) } };
                var dialog = await DialogService.ShowAsync<ConfirmBlockDialog>(Localizer[Langs.DeleteUser], parameters);
                var confirmation = await dialog.Result;
                if (confirmation != null && !confirmation.Canceled)
                {
                    var result = await UserService.BlockUser(user);
                    if (result == Enums.Result.Success)
                    {
                        Snackbar.Add(Localizer[Langs.BlockedSuccessfully], Severity.Success);
                        await _usersDataGrid.ReloadServerData();
                    }
                    else
                    {
                        Snackbar.Add(Localizer[Langs.BlockingFailed], Severity.Error);
                    }
                }
            }
            finally
            {
                _isBusy = false;
                _loadingBlock[user.Id] = false;
            }
        }
        private async Task UnblockUser(UserDTO user)
        {
            if (!HasModulePermission("Users", PermissionFlags.Edit))
                return;

            if (_isBusy || (_loadingBlock.TryGetValue(user.Id, out var loading) && loading))
                return;

            try
            {
                _isBusy = true;
                _loadingBlock[user.Id] = true;

                var parameters = new DialogParameters<ConfirmBlockDialog> { { x => x.ContentText, string.Format(Localizer[Langs.UnblockUserConfirmation], user.Email) }, { x => x.Unblock, true } };
                var dialog = await DialogService.ShowAsync<ConfirmBlockDialog>(Localizer[Langs.DeleteUser], parameters);
                var confirmation = await dialog.Result;
                if (confirmation != null && !confirmation.Canceled)
                {
                    var result = await UserService.BlockUser(user, unblock: true);
                    if (result == Enums.Result.Success)
                    {
                        Snackbar.Add(Localizer[Langs.UnblockedSuccessfully], Severity.Success);
                        await _usersDataGrid.ReloadServerData();
                    }
                    else
                    {
                        Snackbar.Add(Localizer[Langs.UnblockingFailed], Severity.Error);
                    }
                }
            }
            finally
            {
                _isBusy = false;
                _loadingBlock[user.Id] = false;
            }
        }
        private async Task BlockUsers()
        {
            if (!HasModulePermission("Users", PermissionFlags.Edit))
                return;

            if (_isBusy || _loadingBlockMany)
                return;

            var users = _usersDataGrid?.Selection?.ToList();
            if (users == null || !users.Any())
                return;

            if (users.Count == 1)
            {
                var user = users.FirstOrDefault();
                await BlockUser(user);
                return;
            }

            try
            {
                _isBusy = true;
                _loadingBlockMany = true;

                var parameters = new DialogParameters<ConfirmBlockDialog> { { x => x.ContentText, string.Format(Localizer[Langs.BlockUsersConfirmation], users.Count) } };
                var dialog = await DialogService.ShowAsync<ConfirmBlockDialog>(Localizer[Langs.DeleteUser], parameters);
                var confirmation = await dialog.Result;
                if (confirmation != null && !confirmation.Canceled)
                {
                    var result = await UserService.BlockUsers(users);
                    if (result == Enums.Result.Success)
                    {
                        Snackbar.Add(Localizer[Langs.BlockedSuccessfully], Severity.Success);
                        _usersDataGrid.Selection.Clear();
                        await _usersDataGrid.ReloadServerData();
                    }
                    else
                    {
                        Snackbar.Add(Localizer[Langs.BlockingFailed], Severity.Error);
                    }
                }
            }
            finally
            {
                _isBusy = false;
                _loadingBlockMany = false;
            }
        }
        private async Task UnblockUsers()
        {
            if (!HasModulePermission("Users", PermissionFlags.Edit))
                return;

            if (_isBusy || _loadingUnblockMany)
                return;

            var users = _usersDataGrid?.Selection?.ToList();
            if (users == null || !users.Any())
                return;

            if (users.Count == 1)
            {
                var user = users.FirstOrDefault();
                await UnblockUser(user);
                return;
            }

            try
            {
                _isBusy = true;
                _loadingUnblockMany = true;

                var parameters = new DialogParameters<ConfirmBlockDialog> { { x => x.ContentText, string.Format(Localizer[Langs.UnblockUsersConfirmation], users.Count) }, { x => x.Unblock, true } };
                var dialog = await DialogService.ShowAsync<ConfirmBlockDialog>(Localizer[Langs.DeleteUser], parameters);
                var confirmation = await dialog.Result;
                if (confirmation != null && !confirmation.Canceled)
                {
                    var result = await UserService.BlockUsers(users, unblock: true);
                    if (result == Enums.Result.Success)
                    {
                        Snackbar.Add(Localizer[Langs.UnblockedSuccessfully], Severity.Success);
                        _usersDataGrid.Selection.Clear();
                        await _usersDataGrid.ReloadServerData();
                    }
                    else
                    {
                        Snackbar.Add(Localizer[Langs.UnblockingFailed], Severity.Error);
                    }
                }
            }
            finally
            {
                _isBusy = false;
                _loadingUnblockMany = false;
            }
        }


        private Dictionary<Guid, bool> _loadingDelete = [];
        private bool _loadingDeleteMany { get; set; }
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

                var parameters = new DialogParameters<ConfirmDeleteDialog> { { x => x.ContentText, string.Format(Localizer[Langs.DeleteUserConfirmation], user.Email) } };
                var dialog = await DialogService.ShowAsync<ConfirmDeleteDialog>(Localizer[Langs.DeleteUser], parameters);
                var confirmation = await dialog.Result;
                if (confirmation != null && !confirmation.Canceled)
                {
                    var result = await UserService.DeleteUser(user);
                    if (result == Enums.Result.Success)
                    {
                        Snackbar.Add(Localizer[Langs.DeleteSuccess], Severity.Success);
                        await _usersDataGrid.ReloadServerData();
                    }
                    else
                    {
                        Snackbar.Add(Localizer[Langs.DeleteFailed], Severity.Error);
                    }
                }
            }
            finally
            {
                _isBusy = false;
                _loadingDelete[user.Id] = false;
            }
        }
        private async Task DeleteUsers()
        {
            if (!HasModulePermission("Users", PermissionFlags.Delete))
                return;

            if (_isBusy || _loadingDeleteMany)
                return;

            var users = _usersDataGrid?.Selection?.ToList();
            if (users == null || !users.Any())
                return;

            if (users.Count == 1)
            {
                var user = users.FirstOrDefault();
                await DeleteUser(user);
                return;
            }

            try
            {
                _isBusy = true;
                _loadingDeleteMany = true;

                var parameters = new DialogParameters<ConfirmDeleteDialog> { { x => x.ContentText, string.Format(Localizer[Langs.DeleteUsersConfirmation], users.Count) } };
                var dialog = await DialogService.ShowAsync<ConfirmDeleteDialog>(Localizer[Langs.DeleteUser], parameters);
                var confirmation = await dialog.Result;
                if (confirmation != null && !confirmation.Canceled)
                {
                    var result = await UserService.DeleteUsers(users);
                    if (result == Enums.Result.Success)
                    {
                        Snackbar.Add(Localizer[Langs.DeleteSuccess], Severity.Success);
                        _usersDataGrid.Selection.Clear();
                        await _usersDataGrid.ReloadServerData();
                    }
                    else
                    {
                        Snackbar.Add(Localizer[Langs.DeleteFailed], Severity.Error);
                    }
                }
            }
            finally
            {
                _isBusy = false;
                _loadingDeleteMany = false;
            }
        }
    }
}
