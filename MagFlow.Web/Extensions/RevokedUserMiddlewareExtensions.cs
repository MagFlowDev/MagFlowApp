using MagFlow.Web.Middlewares;

namespace MagFlow.Web.Extensions
{
    public static class RevokedUserMiddlewareExtensions
    {
        public static IApplicationBuilder UseRevokedUserMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RevokedUserMiddleware>();
        }
    }
}
