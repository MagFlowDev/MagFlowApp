using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.BLL.Services.Interfaces
{
    public interface IProductService
    {
        Task<QueryResponse<ProductDTO>> GetProducts(int pageNumber = 1, int pageSize = 25, string? search = null, string? sortBy = null, bool descending = false);
        Task<QueryResponse<ProductTypeDTO>> GetTypes(int pageNumber = 1, int pageSize = 25, string? search = null, string? sortBy = null, bool descending = false);
        Task<QueryResponse<ProductCategoryDTO>> GetCategories(int pageNumber = 1, int pageSize = 25, string? search = null, string? sortBy = null, bool descending = false);
        Task<QueryResponse<ProductParameterDTO>> GetParameters(int pageNumber = 1, int pageSize = 25, string? search = null, string? sortBy = null, bool descending = false);
        Task<QueryResponse<UnitDTO>> GetUnits(int pageNumber = 1, int pageSize = 25, string? search = null, string? sortBy = null, bool descending = false);
    }
}
