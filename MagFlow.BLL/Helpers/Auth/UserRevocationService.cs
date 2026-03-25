using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.BLL.Helpers.Auth
{
    public interface IUserRevocationService
    {
        void RevokeUser(string userId, TimeSpan? duration = null);
        bool IsRevoked(string userId);
        void UnrevokeUser(string userId);
    }

    public class UserRevocationService : IUserRevocationService
    {
        private readonly IMemoryCache _cache;

        public UserRevocationService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void RevokeUser(string userId, TimeSpan? duration = null)
        {
            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = duration ?? TimeSpan.FromHours(1)
            };

            _cache.Set($"revoked_{userId}", true, options);
        }

        public bool IsRevoked(string userId)
        {
            return _cache.TryGetValue($"revoked_{userId}", out _);
        }

        public void UnrevokeUser(string userId)
        {
            if (IsRevoked(userId))
                _cache.Remove($"revoked_{userId}");
        }
    }
}
