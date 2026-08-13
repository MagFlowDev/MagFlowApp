using MagFlow.Domain.CompanyScope;
using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.FormModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.BLL.Services.Interfaces
{
    public interface IProductService : IBaseCompanyService<Product, ProductDTO>
    {
        Task<ProductDTO?> GetProduct(int id);

        Task<QueryResponse<ProductDTO>> GetProducts(QueryOptions<Product> options);
        Task<QueryResponse<ProductTypeDTO>> GetTypes(QueryOptions<ProductType> options, ProductCategoryDTO? productCategory = null);
        Task<QueryResponse<ProductCategoryDTO>> GetCategories(QueryOptions<ProductCategory> options);
        Task<QueryResponse<ParameterDTO>> GetParameters(QueryOptions<CustomParameter> options);
        Task<QueryResponse<UnitDTO>> GetUnits(QueryOptions<Unit> options, bool searchRelated = false);

        Task<Enums.Result> AddProduct(ProductFormModel model);
        Task<Enums.Result> AddProductUnitConversion(ProductDTO productDTO, UnitConversionDTO unitConversionDTO);
        Task<Enums.Result> AddType(ProductTypeFormModel model);
        Task<Enums.Result> AddCategory(ProductCategoryFormModel model);
        Task<Enums.Result> AddParameter(ParameterFormModel model);
        Task<Enums.Result> AddMeasurementUnit(MeasurementUnitFormModel model);

        Task<Enums.Result> UpdateProduct(ProductDTO productDTO);
        Task<Enums.Result> UpdateProductUnitConversion(ProductDTO productDTO, UnitConversionDTO unitConversionDTO);
        Task<Enums.Result> UpdateType(ProductTypeDTO typeDTO);
        Task<Enums.Result> UpdateCategory(ProductCategoryDTO categoryDTO);
        Task<Enums.Result> UpdateParameter(ParameterDTO parameterDTO);
        Task<Enums.Result> UpdateMeasurementUnit(UnitDTO unitDTO, List<int>? removedUnits = null);

        Task<Enums.Result> UpdateProductParameters(ProductDTO product, List<ParameterDTO> parametersToAdd, List<ParameterDTO> parametersToRemove);
        Task<Enums.Result> UpdateProductComponents(ProductDTO product, List<ComponentDTO> componentsToAdd, List<ComponentDTO> componentsToRemove);


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
        Task<Enums.Result> DeleteProductUnitsConversions(ProductDTO productDTO, List<UnitConversionDTO> unitConversionsToRemove);
    }
}
