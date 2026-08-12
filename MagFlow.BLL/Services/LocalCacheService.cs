using Blazored.LocalStorage;
using MagFlow.BLL.Helpers;
using MagFlow.BLL.Mappers;
using MagFlow.BLL.Services.Interfaces;
using MagFlow.Domain.CompanyScope;
using MagFlow.Shared.Constants.Identificators;
using MagFlow.Shared.DTOs;
using MagFlow.Shared.DTOs.CoreScope;
using MagFlow.Shared.Models;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using MudBlazor;
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

        public async Task<Enums.Result> SetCurrentModule(Guid sessionId, Guid moduleId)
        {
            try
            {
                var userId = _networkService.GetUserId();
                if (!userId.HasValue)
                    return Enums.Result.Error;
                var cache = await GetCache<List<SessionCurrentModule>>(userId.Value, Shared.Constants.LocalStorageKeys.SESSION_CURRENT_MODULE);
                if (cache == null)
                {
                    await AddOrUpdateCache(userId.Value, Shared.Constants.LocalStorageKeys.SESSION_CURRENT_MODULE, new List<SessionCurrentModule>());
                    cache = await GetCache<List<SessionCurrentModule>>(userId.Value, Shared.Constants.LocalStorageKeys.SESSION_CURRENT_MODULE);
                    if (cache == null)
                        return Enums.Result.Error;
                }

                var sessionCache = new SessionCurrentModule()
                {
                    SessionId = sessionId,
                    ModuleId = moduleId,
                    LastUpdateDate = DateTime.UtcNow,
                };
                if (cache.Any(x => x.SessionId == sessionId))
                {
                    var oldSessionCache = cache.FirstOrDefault(x => x.SessionId == sessionId)!;
                    cache.Remove(oldSessionCache);
                }
                cache.Add(sessionCache);

                if (cache.Count > 5)
                {
                    var oldSessionCache = cache.OrderByDescending(x => x.LastUpdateDate).Skip(5).ToList();
                    foreach (var toRemove in oldSessionCache)
                        cache.Remove(toRemove);
                }
                await AddOrUpdateCache(userId.Value, Shared.Constants.LocalStorageKeys.SESSION_CURRENT_MODULE, cache);

                return Enums.Result.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while saving session current module in local storage");
                return Enums.Result.Error;
            }
        }

        public async Task<Guid?> GetCurrentModule(Guid sessionId)
        {
            try
            {
                var userId = _networkService.GetUserId();
                if (!userId.HasValue)
                    return null;

                var cache = await GetCache<List<SessionCurrentModule>>(userId.Value, Shared.Constants.LocalStorageKeys.SESSION_CURRENT_MODULE);
                if (cache == null)
                    return null;

                var sessionCache = cache.FirstOrDefault(x => x.SessionId == sessionId);
                if (sessionCache != null)
                {
                    sessionCache.LastUpdateDate = DateTime.UtcNow;
                    await AddOrUpdateCache(userId.Value, Shared.Constants.LocalStorageKeys.SESSION_CURRENT_MODULE, cache);
                }

                return sessionCache?.ModuleId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while getting session current module from local storage");
                return null;
            }
        }

        public async Task<Enums.Result> SetCurrentModuleSection(Guid sessionId, Guid moduleId, Enum section)
        {
            try
            {
                var userId = _networkService.GetUserId();
                if (!userId.HasValue)
                    return Enums.Result.Error;
                var cache = await GetCache<List<SessionModuleSection>>(userId.Value, Shared.Constants.LocalStorageKeys.SESSION_MODULE_SECTION);
                if (cache == null)
                {
                    await AddOrUpdateCache(userId.Value, Shared.Constants.LocalStorageKeys.SESSION_MODULE_SECTION, new List<SessionModuleSection>());
                    cache = await GetCache<List<SessionModuleSection>>(userId.Value, Shared.Constants.LocalStorageKeys.SESSION_MODULE_SECTION);
                    if (cache == null)
                        return Enums.Result.Error;
                }

                var sessionCache = new SessionModuleSection()
                {
                    SessionId = sessionId,
                    ModuleId = moduleId,
                    Section = section.ToString(),
                    SectionType = section.GetType().AssemblyQualifiedName,
                    LastUpdateDate = DateTime.UtcNow,
                };
                if (cache.Any(x => x.SessionId == sessionId && x.ModuleId == moduleId))
                {
                    var oldSessionCache = cache.FirstOrDefault(x => x.SessionId == sessionId && x.ModuleId == moduleId)!;
                    cache.Remove(oldSessionCache);
                }
                cache.Add(sessionCache);

                if (cache.Count > 20)
                {
                    var oldSessionCache = cache.OrderByDescending(x => x.LastUpdateDate).Skip(20).ToList();
                    foreach (var toRemove in oldSessionCache)
                        cache.Remove(toRemove);
                }
                await AddOrUpdateCache(userId.Value, Shared.Constants.LocalStorageKeys.SESSION_MODULE_SECTION, cache);

                return Enums.Result.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while saving session module section in local storage");
                return Enums.Result.Error;
            }
        }

        public async Task<Enum?> GetCurrentModuleSection(Guid sessionId, Guid moduleId)
        {
            try
            {
                var userId = _networkService.GetUserId();
                if (!userId.HasValue)
                    return null;

                var cache = await GetCache<List<SessionModuleSection>>(userId.Value, Shared.Constants.LocalStorageKeys.SESSION_MODULE_SECTION);
                if (cache == null)
                    return null;

                var sessionCache = cache.FirstOrDefault(x => x.SessionId == sessionId && x.ModuleId == moduleId);
                if (sessionCache != null)
                {
                    sessionCache.LastUpdateDate = DateTime.UtcNow;
                    await AddOrUpdateCache(userId.Value, Shared.Constants.LocalStorageKeys.SESSION_MODULE_SECTION, cache);
                }

                if (sessionCache == null)
                    return null;

                var section = EnumsHelper.ParseEnum(sessionCache.SectionType, sessionCache.Section);
                return section != null ? (Enum)section : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while getting session module section from local storage");
                return null;
            }
        }

        public async Task<Enums.Result> SetTableFilters<T>(Guid sessionId, string tableId, bool filtersDisplayed, List<IFilterDefinition<T>> filters)
        {
            try
            {
                var userId = _networkService.GetUserId();
                if (!userId.HasValue)
                    return Enums.Result.Error;
                var cache = await GetCache<List<SessionTableFilters>>(userId.Value, Shared.Constants.LocalStorageKeys.SESSION_TABLE_FILTERS);
                if (cache == null)
                {
                    await AddOrUpdateCache(userId.Value, Shared.Constants.LocalStorageKeys.SESSION_TABLE_FILTERS, new List<SessionTableFilters>());
                    cache = await GetCache<List<SessionTableFilters>>(userId.Value, Shared.Constants.LocalStorageKeys.SESSION_TABLE_FILTERS);
                    if (cache == null)
                        return Enums.Result.Error;
                }

                var sessionCache = new SessionTableFilters()
                {
                    SessionId = sessionId,
                    TableId = tableId,
                    Filters = filters.ToDTO(),
                    FiltersDisplayed = filtersDisplayed,
                    LastUpdateDate = DateTime.UtcNow,
                };
                if (cache.Any(x => x.SessionId == sessionId && x.TableId == tableId))
                {
                    var oldSessionCache = cache.FirstOrDefault(x => x.SessionId == sessionId && x.TableId == tableId)!;
                    cache.Remove(oldSessionCache);
                }
                cache.Add(sessionCache);

                if (cache.Count > 20)
                {
                    var oldSessionCache = cache.OrderByDescending(x => x.LastUpdateDate).Skip(20).ToList();
                    foreach (var toRemove in oldSessionCache)
                        cache.Remove(toRemove);
                }
                await AddOrUpdateCache(userId.Value, Shared.Constants.LocalStorageKeys.SESSION_TABLE_FILTERS, cache);

                return Enums.Result.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while saving session table filters in local storage");
                return Enums.Result.Error;
            }
        }

        public async Task<(List<IFilterDefinition<T>>? filters, bool filtersDisplayed)> GetTableFilters<T>(Guid sessionId, string tableId, MudDataGrid<T> dataGrid)
        {
            try
            {
                var userId = _networkService.GetUserId();
                if (!userId.HasValue)
                    return (null, false);

                var cache = await GetCache<List<SessionTableFilters>>(userId.Value, Shared.Constants.LocalStorageKeys.SESSION_TABLE_FILTERS);
                if (cache == null)
                    return (null, false);

                var sessionCache = cache.FirstOrDefault(x => x.SessionId == sessionId && x.TableId == tableId);
                if (sessionCache != null)
                {
                    sessionCache.LastUpdateDate = DateTime.UtcNow;
                    await AddOrUpdateCache(userId.Value, Shared.Constants.LocalStorageKeys.SESSION_TABLE_FILTERS, cache);
                }

                if (sessionCache == null)
                    return (null, false);

                var filtersDTOs = sessionCache.Filters;
                var filters = filtersDTOs?.ToEntity(dataGrid).Where(x => x != null).Select(x => x!).ToList();
                return (filters, sessionCache.FiltersDisplayed);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while getting session table filters from local storage");
                return (null, false);
            }
        }


        public async Task<Enums.Result> Copy(object obj)
        {
            try
            {
                var userId = _networkService.GetUserId();
                if (!userId.HasValue)
                    return Enums.Result.Error;
                await AddOrUpdateCache(userId.Value, Shared.Constants.LocalStorageKeys.CLIPBOARD, obj);

                return Enums.Result.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while copying object to local storage");
                return Enums.Result.Error;
            }
        }

        public async Task<object> Paste()
        {
            try
            {
                var userId = _networkService.GetUserId();
                if (!userId.HasValue)
                    return null;

                var cache = await GetCache<object>(userId.Value, Shared.Constants.LocalStorageKeys.CLIPBOARD);
                if (cache == null)
                    return null;

                return cache;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while getting copied object from local storage");
                return null;
            }
        }

        public async Task<Enums.Result> CopyItem(object obj, string type)
        {
            try
            {
                var userId = _networkService.GetUserId();
                if (!userId.HasValue)
                    return Enums.Result.Error;
                var expire = DateTime.UtcNow.AddSeconds(30);    
                await AddOrUpdateCache(userId.Value, Shared.Constants.LocalStorageKeys.ITEM_TEMP_CLIPBOARD, obj, type, expire);

                return Enums.Result.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while copying item to local storage");
                return Enums.Result.Error;
            }
        }

        public async Task<(object, string)> PasteItem()
        {
            try
            {
                var userId = _networkService.GetUserId();
                if (!userId.HasValue)
                    return (null, null);

                var cache = await GetCacheWithType<object>(userId.Value, Shared.Constants.LocalStorageKeys.ITEM_TEMP_CLIPBOARD);
                var data = cache.Item1;
                var type = cache.Item2;
                if (data == null)
                    return (null, null);

                await RemoveCache(userId.Value, Shared.Constants.LocalStorageKeys.ITEM_TEMP_CLIPBOARD);
                return (data,type);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while getting copied item from local storage");
                return (null, null);
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

        private async Task<(T?, string)> GetCacheWithType<T>(Guid userId, string key)
        {
            var storageKey = string.Concat(userId.ToString(), "_", key);
            var cache = await _localStorage.GetItemAsync<StorageItem<T>>(storageKey);
            if (cache != null)
            {
                if(cache.Expire.HasValue && cache.Expire.Value < DateTime.UtcNow)
                {
                    await RemoveCache(userId, key);
                    return (default(T), null);
                }
                return (cache.Data, cache.Type);
            }
            else
                return (default(T), null);
        }

        private async Task<T?> GetCache<T>(string key)
        {
            var cache = await _localStorage.GetItemAsync<StorageItem<T>>(key);
            if (cache != null)
                return cache.Data;
            else
                return default(T);
        }

        private async Task<(T?, string)> GetCacheWithType<T>(string key)
        {
            var cache = await _localStorage.GetItemAsync<StorageItem<T>>(key);
            if (cache != null)
            {
                if (cache.Expire.HasValue && cache.Expire.Value < DateTime.UtcNow)
                {
                    await RemoveCache(key);
                    return (default(T), null);
                }
                return (cache.Data, cache.Type);
            }
            else
                return (default(T), null);
        }

        private async Task AddOrUpdateCache<T>(Guid userId, string key, T data, string? type = null, DateTime? expiration = null)
        {
            var storageKey = string.Concat(userId.ToString(), "_", key);
            StorageItem<T> item = new StorageItem<T>() { Key = storageKey, Data = data, Type = type, Expire = expiration };
            await _localStorage.SetItemAsync(item.Key, item);
        }

        private async Task AddOrUpdateCache<T>(string key, T data, string? type = null, DateTime? expiration = null)
        {
            StorageItem<T> item = new StorageItem<T>() { Key = key, Data = data, Type = type, Expire = expiration };
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
