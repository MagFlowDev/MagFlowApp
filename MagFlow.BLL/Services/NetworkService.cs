using MagFlow.BLL.Services.Interfaces;
using MagFlow.Shared.Extensions;
using MagFlow.Shared.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
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
        private readonly HttpClient _httpClient;

        public NetworkService(IHttpContextAccessor httpContextAccessor, HttpClient httpClient)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClient = httpClient;
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

        public async Task<Enums.Result> SendPost<TRequest>(TRequest request, string address)
        {
            try
            {
                var json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(address, content);
                if (response.IsSuccessStatusCode)
                    return Enums.Result.Success;
                else
                    return Enums.Result.Error;
            }
            catch (Exception)
            {
                return Enums.Result.Error;
            }
        }
    }
}
