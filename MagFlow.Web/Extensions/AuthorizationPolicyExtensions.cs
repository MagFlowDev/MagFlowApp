using MagFlow.BLL.Security.Requirements;
using MagFlow.Shared.Models.Enumerators;
using Microsoft.AspNetCore.Authorization;
using System.Reflection;
using System.Security.Claims;

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

            options.AddPolicy(Shared.Constants.Policies.USER_ADD_OR_ADMIN, policy =>
                policy.Requirements.Add(new RoleOrPermissionRequirement("SuperAdmin", "User.Edit")));

            options.AddClaimBasedPolicies();

            return options;
        }

        public static AuthorizationOptions AddClaimBasedPolicies(this AuthorizationOptions options)
        {
            var claimDictionary = typeof(MagFlow.Shared.Constants.Policies.Claims)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(f => f.IsLiteral && !f.IsInitOnly)
                .ToDictionary(
                    field => field.Name,
                    field => field.GetValue(null)?.ToString()
                );
            if (claimDictionary == null)
                return options;

            foreach(var claim in claimDictionary)
            {
                if (string.IsNullOrEmpty(claim.Value))
                    continue;
                options.AddPolicy(claim.Value, policy =>
                    policy.Requirements.Add(new RoleOrPermissionRequirement("SuperAdmin", claim.Value)));
            }

            return options;
        }
    }
}
