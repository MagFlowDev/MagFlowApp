using MagFlow.BLL.Security.Handlers;
using MagFlow.DAL.Repositories.CoreScope.Interfaces;
using MagFlow.Shared.Extensions;
using MagFlow.Shared.Models.Auth;
using MagFlow.Shared.Models.Enumerators;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;
using System.Security.Claims;

namespace MagFlow.Web.Pages.Modules
{
    public abstract class AuthComponentBase : ComponentBase, IDisposable
    {
        [Inject] protected AuthenticationStateProvider AuthStateProvider { get; set; } = default!;

        private readonly ConcurrentDictionary<string, long> _moduleMasks = new(StringComparer.OrdinalIgnoreCase);
        protected string? _userRole;
        protected ClaimsPrincipal? _currentUser;
        protected Guid? _currentCompanyId;

        private bool _initialized;
        private bool _disposed;

        protected override void OnInitialized()
        {
            AuthStateProvider.AuthenticationStateChanged += OnAuthenticationStateChanged;
            _ = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            var state = await AuthStateProvider.GetAuthenticationStateAsync();
            UpdateFromPrincipal(state.User);
            _initialized = true;
            StateHasChanged();
        }

        private async void OnAuthenticationStateChanged(Task<AuthenticationState> task)
        {
            try
            {
                var state = await task;
                UpdateFromPrincipal(state.User);
                await InvokeAsync(StateHasChanged);
            }
            catch
            {
                
            }
        }

        private void UpdateFromPrincipal(ClaimsPrincipal? user)
        {
            _moduleMasks.Clear();
            _currentUser = user;

            if (user == null || user.Identity?.IsAuthenticated != true)
                return;

            _currentCompanyId = user.FindFirst(Claims.CompanyClaim)?.Value.ToGuid();
            _userRole = user.FindFirst(ClaimTypes.Role)?.Value;

            var moduleClaims = user.Claims
                .Where(c => c.Type != null && c.Type.StartsWith("perms:", StringComparison.OrdinalIgnoreCase))
                .ToArray();

            foreach (var c in moduleClaims)
            {
                var module = c.Type.Substring("perms:".Length);
                if (long.TryParse(c.Value, out var mask))
                    _moduleMasks[module] = mask;
            }
        }

        protected bool HasModulePermission(string moduleCode, PermissionFlags required)
        {
            if (_currentUser?.IsInRole(nameof(AppRole.SuperAdmin)) == true)
                return true;
            if (_moduleMasks.TryGetValue(moduleCode, out var mask))
                return (((PermissionFlags)mask) & required) == required;
            return false;
        }

        protected bool HasAnyPermissionAcrossModules(PermissionFlags anyOf)
        {
            if (_currentUser?.IsInRole(nameof(AppRole.SuperAdmin)) == true)
                return true;
            foreach (var kv in _moduleMasks)
                if ((((PermissionFlags)kv.Value) & anyOf) != PermissionFlags.None) return true;
            return false;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                AuthStateProvider.AuthenticationStateChanged -= OnAuthenticationStateChanged;
                _disposed = true;
            }
        }
    }
}
