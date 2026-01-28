using Castle.Core;
using Castle.DynamicProxy;
using MagFlow.Shared.Attributes;
using Microsoft.AspNetCore.Components.Authorization;
namespace MagFlow.BLL.Helpers.Auth
{
    public class SecurityInterceptor : AsyncInterceptorBase, IInterceptor
    {
        private readonly AuthenticationStateProvider _authStateProvider;

        public SecurityInterceptor(AuthenticationStateProvider authStateProvider)
        {
            _authStateProvider = authStateProvider;
        }

        public void Intercept(IInvocation invocation)
        {
            throw new Exception("Method must be async!");
        }

        protected override async Task InterceptAsync(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task> proceed)
        {
            await CheckPermissions(invocation);
            await proceed(invocation, proceedInfo);
        }

        protected override async Task<TResult> InterceptAsync<TResult>(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task<TResult>> proceed)
        {
            await CheckPermissions(invocation);
            return await proceed(invocation, proceedInfo);
        }

        

        private async Task CheckPermissions(IInvocation invocation)
        {
            var attribute = invocation?.MethodInvocationTarget?
               .GetCustomAttributes(typeof(MinimumRoleAttribute), true)
               .FirstOrDefault() as MinimumRoleAttribute;

            if (attribute != null)
            {
                var authState = await _authStateProvider.GetAuthenticationStateAsync();
                var user = authState.User;

                if(!user.IsInRole(attribute.Role))
                {
                    throw new UnauthorizedAccessException($"Access denied: Requires {attribute.Role}");
                }
            }
        }
    }
}
