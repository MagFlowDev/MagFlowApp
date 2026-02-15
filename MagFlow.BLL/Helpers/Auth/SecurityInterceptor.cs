using Castle.Core;
using Castle.DynamicProxy;
using MagFlow.BLL.Services.Interfaces;
using MagFlow.DAL.Repositories.Core.Interfaces;
using MagFlow.Shared.Attributes;
using MagFlow.Shared.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
namespace MagFlow.BLL.Helpers.Auth
{
    public class SecurityInterceptor : AsyncInterceptorBase, IInterceptor
    {
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly IUserRepository _userRepository;

        public SecurityInterceptor(AuthenticationStateProvider authStateProvider,
            IUserRepository userRepository)
        {
            _authStateProvider = authStateProvider;
            _userRepository = userRepository;
        }

        public void Intercept(IInvocation invocation)
        {
            throw new Exception("Method must be async!");
        }

        protected override async Task InterceptAsync(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task> proceed)
        {
            if (await CheckPermissions(invocation))
            {
                await proceed(invocation, proceedInfo);
            }
        }

        protected override async Task<TResult> InterceptAsync<TResult>(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task<TResult>> proceed)
        {
            if (await CheckPermissions(invocation))
            {
                return await proceed(invocation, proceedInfo);
            }
            return default!;
        }

        

        private async Task<bool> CheckPermissions(IInvocation invocation)
        {
            var minimumAttribute = invocation?.MethodInvocationTarget?
               .GetCustomAttributes(typeof(MinimumRoleAttribute), true)
               .FirstOrDefault() as MinimumRoleAttribute;
            var mustHaveAttribute = invocation?.MethodInvocationTarget?
               .GetCustomAttributes(typeof(MustHaveRoleAttribute), true)
               .FirstOrDefault() as MustHaveRoleAttribute;

            bool hasAccess = true;
            if (minimumAttribute != null)
                hasAccess = await CheckMinimumAttribute(minimumAttribute);
            if (hasAccess == false)
                return hasAccess;
            if (mustHaveAttribute != null)
                hasAccess = await CheckMustHaveAttribute(mustHaveAttribute);
            return hasAccess;
        }

        private async Task<bool> CheckMinimumAttribute(MinimumRoleAttribute attribute)
        {
            if (attribute != null)
            {
                var authState = await _authStateProvider.GetAuthenticationStateAsync();
                var user = authState.User;
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value.ToGuid();
                if (!userId.HasValue)
                {
                    return false;
                    //throw new UnauthorizedAccessException($"User not authorized, action requires minimum {attribute.Role}");
                }
                var userApp = await _userRepository.GetByIdAsync(userId.Value);
                var roles = userApp?.Roles.Where(r => r.Role?.Name != null).Select(r => r.Role!.Name!).ToList();
                if (!RoleAuthorizationHelper.HasMinimumRole(attribute.Role, roles ?? new List<string>()))
                {
                    return false;
                    //throw new UnauthorizedAccessException($"User not authorized, action requires minimum {attribute.Role}");
                }
                else
                    return true;
            }
            else
                return true;
        }

        private async Task<bool> CheckMustHaveAttribute(MustHaveRoleAttribute attribute)
        {
            if (attribute != null)
            {
                var authState = await _authStateProvider.GetAuthenticationStateAsync();
                var user = authState.User;
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value.ToGuid();
                if (!userId.HasValue)
                {
                    return false;
                    //throw new UnauthorizedAccessException($"User not authorized, action requires exactly {attribute.Role}");
                }
                var userApp = await _userRepository.GetByIdAsync(userId.Value);
                var roles = userApp?.Roles.Where(r => r.Role?.Name != null).Select(r => r.Role!.Name!).ToList();
                if (!RoleAuthorizationHelper.HasExactRole(attribute.Role, roles ?? new List<string>()))
                {
                    return false;
                    //throw new UnauthorizedAccessException($"User not authorized, action requires exactly {attribute.Role}");
                }
                else
                    return true;
            }
            else
                return true;
        }
    }
}
