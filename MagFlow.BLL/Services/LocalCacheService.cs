using Blazored.LocalStorage;
using MagFlow.BLL.Services.Interfaces;
using MagFlow.Domain.Company;
using MagFlow.Shared.DTOs.Core;
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
        private readonly INetworkService _networkService;
        private readonly ILogger<LocalCacheService> _logger;

        public LocalCacheService(ILogger<LocalCacheService> logger, 
            ILocalStorageService localStorage,
            INetworkService networkService)
        {
            _logger = logger;
            _localStorage = localStorage;
            _networkService = networkService;
        }

        public async Task<Enums.Result> SetSessionOrder(Guid sessionId, List<Guid> orderedIds)
        {
            try
            {
                var userId = _networkService.GetUserId();
                if (!userId.HasValue)
                    return Enums.Result.Error;
                var cache = await GetCache<List<SessionCache>>(userId.Value, Shared.Constants.LocalStorageKeys.SESSION_ORDER);
                if(cache == null)
                {
                    await AddOrUpdateCache(userId.Value, Shared.Constants.LocalStorageKeys.SESSION_ORDER, new List<SessionCache>());
                    cache = await GetCache<List<SessionCache>>(userId.Value, Shared.Constants.LocalStorageKeys.SESSION_ORDER);
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
                await AddOrUpdateCache(userId.Value, Shared.Constants.LocalStorageKeys.SESSION_ORDER, cache);
                
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
            try
            {
                var userId = _networkService.GetUserId();
                if (!userId.HasValue)
                    return null;

                var cache = await GetCache<List<SessionCache>>(userId.Value, Shared.Constants.LocalStorageKeys.SESSION_ORDER);
                if(cache == null)
                    return null;

                var sessionCache = cache.FirstOrDefault(x => x.SessionId == sessionId);
                if(sessionCache != null)
                {
                    sessionCache.LastUpdateDate = DateTime.UtcNow;
                    await AddOrUpdateCache(userId.Value, Shared.Constants.LocalStorageKeys.SESSION_ORDER, cache);
                }

                return sessionCache?.SessionOrder;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occured while getting session order from local storage");
                return null;
            }
        }

        public async Task<Enums.Result> SetCurrentUser(UserDTO userDTO)
        {
            try
            {
                var userId = _networkService.GetUserId();
                if (!userId.HasValue)
                    return Enums.Result.Error;

                if (userDTO.Id != userId.Value)
                    return Enums.Result.Error;

                var cache = await GetCache<UserDTO>(Shared.Constants.LocalStorageKeys.CURRENT_USER);
                if (cache != null)
                {
                    await RemoveCache(Shared.Constants.LocalStorageKeys.CURRENT_USER);
                }

                await AddOrUpdateCache<UserDTO>(Shared.Constants.LocalStorageKeys.CURRENT_USER, userDTO);
                return Enums.Result.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while saving current user in local storage");
                return Enums.Result.Error;
            }
        }

        public async Task<UserDTO?> GetCurrentUser()
        {
            try
            {
                var userId = _networkService.GetUserId();
                if (!userId.HasValue)
                    return null;

                var cache = await GetCache<UserDTO>(Shared.Constants.LocalStorageKeys.CURRENT_USER);
                if (cache == null)
                    return null;

                if (cache.Id != userId.Value)
                    return null;

                return cache;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while getting current user from local storage");
                return null;
            }
        }



        

        private async Task<T?> GetCache<T>(Guid userId, string key)
        {
            var storageKey = string.Concat(userId.ToString(), "_", key);
            var cache = await _localStorage.GetItemAsync<StorageItem<T>>(storageKey);
            if (cache != null)
                return cache.Data;
            else
                return default(T);
        }

        private async Task<T?> GetCache<T>(string key)
        {
            var cache = await _localStorage.GetItemAsync<StorageItem<T>>(key);
            if (cache != null)
                return cache.Data;
            else
                return default(T);
        }

        private async Task AddOrUpdateCache<T>(Guid userId, string key, T data)
        {
            var storageKey = string.Concat(userId.ToString(), "_", key);
            StorageItem<T> item = new StorageItem<T>() { Key = storageKey, Data = data };
            await _localStorage.SetItemAsync(item.Key, item);
        }

        private async Task AddOrUpdateCache<T>(string key, T data)
        {
            StorageItem<T> item = new StorageItem<T>() { Key = key, Data = data };
            await _localStorage.SetItemAsync(item.Key, item);
        }

        private async Task RemoveCache(Guid userId, string key)
        {
            var storageKey = string.Concat(userId.ToString(), "_", key);
            await _localStorage.RemoveItemAsync(storageKey);
        }

        private async Task RemoveCache(string key)
        {
            await _localStorage.RemoveItemAsync(key);
        }
    }
}
