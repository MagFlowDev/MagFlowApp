using MagFlow.Shared.DTOs;
using MagFlow.Shared.DTOs.CoreScope;
using MagFlow.Shared.Models;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.BLL.Services.Interfaces
{
    public interface ILocalCacheService
    {
        Task<Enums.Result> SetSessionOrder(Guid sessionId, List<Guid> orderedIds);
        Task<List<Guid>?> GetSessionOrder(Guid sessionId);

        Task<Enums.Result> SetCurrentModule(Guid sessionId, Guid moduleId);
        Task<Guid?> GetCurrentModule(Guid sessionId);

        Task<Enums.Result> SetCurrentModuleSection(Guid sessionId, Guid moduleId, Enum section);
        Task<Enum?> GetCurrentModuleSection(Guid sessionId, Guid moduleId);

        Task<Enums.Result> SetTableFilters<T>(Guid sessionId, string tableId, bool filtersDisplayed, List<IFilterDefinition<T>> filters);
        Task<(List<IFilterDefinition<T>>? filters, bool filtersDisplayed)> GetTableFilters<T>(Guid sessionId, string tableId, MudDataGrid<T> dataGrid);

        Task<Enums.Result> SetCurrentUser(UserDTO userDTO);
        Task<UserDTO?> GetCurrentUser();

        Task<Enums.Result> Copy(object obj);
        Task<object> Paste();
        Task<Enums.Result> CopyItem(object obj, string type);
        Task<(object, string)> PasteItem();
    }
}
