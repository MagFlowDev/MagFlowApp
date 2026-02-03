using MagFlow.BLL.Services.Interfaces;
using MagFlow.Shared.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.BLL.Services
{
    public class NetworkService : INetworkService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NetworkService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid? GetUserId()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value.ToGuid();
        }

        public string? GetUserIp()
        {
            if (_httpContextAccessor == null)
                return null;
            var context = _httpContextAccessor.HttpContext;
            if (context == null)
                return null;

            if(context.Request.Headers.TryGetValue("X-Forwarded-For", out var forwardedFor))
            {
                return forwardedFor.FirstOrDefault()?.Split(',').FirstOrDefault()?.Trim();
            }

            return context.Connection.RemoteIpAddress?.ToString();
        }
    }
}
