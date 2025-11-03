using MagFlow.Domain.Core;
using MagFlow.Shared.Models.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;

namespace MagFlow.Web.Helpers
{
    public static class AppEndpointMapper
    {
        public static void MapEndpoints(this WebApplication app)
        {
            app.MapLogin();
        }

        private static void MapLogin(this WebApplication app)
        {
            
        }
    }
}
