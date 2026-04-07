using System.Security.Claims;
using MagFlow.Shared.Models.Enumerators;
using Microsoft.AspNetCore.Authorization;

namespace MagFlow.Web.Extensions
{
    public static class AuthorizationPolicyExtensions
    {
        // https://medium.com/c-sharp-programming/claims-based-authentication-dotnet-core-guide-245f099a872b

        public static AuthorizationOptions AddPolicies(this AuthorizationOptions options)
        {
            options.AddPolicy(Shared.Constants.Policies.ADMIN_ONLY, policy =>
                policy.RequireClaim(ClaimTypes.Role, nameof(AppRole.SysAdmin), nameof(AppRole.SuperAdmin)));

            options.AddPolicy(Shared.Constants.Policies.COMPANY_ADMIN, policy =>
                policy.RequireRole(nameof(AppRole.SysAdmin), nameof(AppRole.SuperAdmin), nameof(AppRole.CompanyAdmin)));

            options.AddPolicy(Shared.Constants.Policies.CAN_ACCESS_USERS, policy => 
                policy.RequireAssertion(context =>
                {
                    var user = context.User;

                    if (user.HasClaim(ClaimTypes.Role, nameof(AppRole.CompanyAdmin)) ||
                        user.HasClaim(ClaimTypes.Role, nameof(AppRole.SysAdmin)) ||
                        user.HasClaim(ClaimTypes.Role, nameof(AppRole.SuperAdmin)))
                        return true;

                    return false;
                }));

            return options;
        }
    }
}
