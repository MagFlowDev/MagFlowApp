using Blazored.LocalStorage;
using MagFlow.BLL.Services.Interfaces;
using MagFlow.Shared.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.BLL.Services
{
    public class LocalCacheService : ILocalCacheService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly ILogger<LocalCacheService> _logger;

        public LocalCacheService(ILogger<LocalCacheService> logger, 
            ILocalStorageService localStorage)
        {
            _logger = logger;
            _localStorage = localStorage;
        }

        public async Task<Enums.Result> SetSessionOrder(Guid sessionId, List<Guid> orderedIds)
        {
            try
            {
                var cache = await GetCache<List<SessionCache>>(Shared.Constants.LocalStorageKeys.SESSION_ORDER);
                if(cache == null)
                {
                    await AddOrUpdateCache(Shared.Constants.LocalStorageKeys.SESSION_ORDER, new List<SessionCache>());
                    cache = await GetCache<List<SessionCache>>(Shared.Constants.LocalStorageKeys.SESSION_ORDER);
                    if (cache == null)
                        return Enums.Result.Error;
                }

                var sessionCache = new SessionCache()
                {
                    SessionId = sessionId,
                    SessionOrder = orderedIds,
                    LastUpdateDate = DateTime.UtcNow
                };
                if(cache.Any(x => x.SessionId == sessionId))
                {
                    var oldSessionCache = cache.FirstOrDefault(x => x.SessionId == sessionId)!;
                    cache.Remove(oldSessionCache);
                }
                cache.Add(sessionCache);

                if(cache.Count > 5)
                {
                    var oldSessionCache = cache.OrderByDescending(x => x.LastUpdateDate).Skip(5).ToList();
                    foreach (var toRemove in oldSessionCache)
                        cache.Remove(toRemove);
                }
                await AddOrUpdateCache(Shared.Constants.LocalStorageKeys.SESSION_ORDER, cache);
                
                return Enums.Result.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while saving session order in local storage");
                return Enums.Result.Error;
            }
        }

        public async Task<List<Guid>?> GetSessionOrder(Guid sessionId)
        {
            return null;
        }



        

        private async Task<T?> GetCache<T>(string key)
        {
            var cache = await _localStorage.GetItemAsync<StorageItem<T>>(key);
            if (cache != null)
                return cache.Data;
            else
                return default(T);
        }

        private async Task AddOrUpdateCache<T>(string key, T data)
        {
            StorageItem<T> item = new StorageItem<T>() { Key = key, Data = data };
            await _localStorage.SetItemAsync(item.Key, item);
        }

        private async Task RemoveCache(string key)
        {
            await _localStorage.RemoveItemAsync(key);
        }
    }
}
