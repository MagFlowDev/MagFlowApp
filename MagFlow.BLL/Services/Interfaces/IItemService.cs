using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.BLL.Services.Interfaces
{
    public interface IItemService
    {
        Task<QueryResponse<ItemDTO>> GetItems(int pageNumber = 1, int pageSize = 25, string? search = null, string? sortBy = null, bool descending = false);
        Task<QueryResponse<ItemDTO>> GetArchive(int pageNumber = 1, int pageSize = 25, string? search = null, string? sortBy = null, bool descending = false);
    }
}
