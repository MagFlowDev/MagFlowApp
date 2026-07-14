using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.BLL.Services.Interfaces
{
    public interface IBaseCompanyService<TEntity> where TEntity : class
    {
        Task<QueryResponse<EntityHistoryDTO>> GetEntityHistory(int id, Enums.HistoryEntityType entityType, int pageNumber = 0, int pageSize = 25, string? search = null, string? sortBy = null, bool descending = false);
    }
}
