using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.FormModels;
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

        Task<Enums.Result> AddProduct(ProductFormModel model);
        Task<Enums.Result> AddType(ProductTypeFormModel model);
        Task<Enums.Result> AddCategory(ProductCategoryFormModel model);
        Task<Enums.Result> AddParameter(ProductParameterFormModel model);
        Task<Enums.Result> AddMeasurementUnit(MeasurementUnitFormModel model);

        Task<Enums.Result> UpdateProduct(ProductDTO productDTO);
        Task<Enums.Result> UpdateType(ProductTypeDTO typeDTO);
        Task<Enums.Result> UpdateCategory(ProductCategoryDTO categoryDTO);
        Task<Enums.Result> UpdateParameter(ProductParameterDTO parameterDTO);
        Task<Enums.Result> UpdateMeasurementUnit(UnitDTO unitDTO);

        Task<Enums.Result> DeleteProduct(ProductDTO productDTO);
        Task<Enums.Result> DeleteType(ProductTypeDTO typeDTO);
        Task<Enums.Result> DeleteCategory(ProductCategoryDTO categoryDTO);
        Task<Enums.Result> DeleteParameter(ProductParameterDTO parameterDTO);
        Task<Enums.Result> DeleteMeasurementUnit(UnitDTO unitDTO);
    }
}
