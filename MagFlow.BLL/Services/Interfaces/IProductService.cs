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
        Task<ProductDTO?> GetProduct(int id);

        Task<QueryResponse<ProductDTO>> GetProducts(int pageNumber = 0, int pageSize = 25, string? search = null, string? sortBy = null, bool descending = false);
        Task<QueryResponse<ProductTypeDTO>> GetTypes(int pageNumber = 0, int pageSize = 25, string? search = null, string? sortBy = null, bool descending = false, ProductCategoryDTO? productCategory = null);
        Task<QueryResponse<ProductCategoryDTO>> GetCategories(int pageNumber = 0, int pageSize = 25, string? search = null, string? sortBy = null, bool descending = false);
        Task<QueryResponse<ParameterDTO>> GetParameters(int pageNumber = 0, int pageSize = 25, string? search = null, string? sortBy = null, bool descending = false);
        Task<QueryResponse<UnitDTO>> GetUnits(int pageNumber = 0, int pageSize = 25, string? search = null, string? sortBy = null, bool descending = false);

        Task<Enums.Result> AddProduct(ProductFormModel model);
        Task<Enums.Result> AddType(ProductTypeFormModel model);
        Task<Enums.Result> AddCategory(ProductCategoryFormModel model);
        Task<Enums.Result> AddParameter(ParameterFormModel model);
        Task<Enums.Result> AddMeasurementUnit(MeasurementUnitFormModel model);

        Task<Enums.Result> UpdateProduct(ProductDTO productDTO);
        Task<Enums.Result> UpdateType(ProductTypeDTO typeDTO);
        Task<Enums.Result> UpdateCategory(ProductCategoryDTO categoryDTO);
        Task<Enums.Result> UpdateParameter(ParameterDTO parameterDTO);
        Task<Enums.Result> UpdateMeasurementUnit(UnitDTO unitDTO, List<int>? removedUnits = null);

        Task<Enums.Result> DeleteProduct(ProductDTO productDTO);
        Task<Enums.Result> DeleteProducts(List<ProductDTO> productDTOs);
        Task<Enums.Result> DeleteType(ProductTypeDTO typeDTO);
        Task<Enums.Result> DeleteTypes(List<ProductTypeDTO> typeDTOs);
        Task<Enums.Result> DeleteCategory(ProductCategoryDTO categoryDTO);
        Task<Enums.Result> DeleteCategories(List<ProductCategoryDTO> categoryDTOs);
        Task<Enums.Result> DeleteParameter(ParameterDTO parameterDTO);
        Task<Enums.Result> DeleteParameters(List<ParameterDTO> parameterDTOs);
        Task<Enums.Result> DeleteMeasurementUnit(UnitDTO unitDTO);
        Task<Enums.Result> DeleteMeasurementUnits(List<UnitDTO> unitDTOs);
    }
}
