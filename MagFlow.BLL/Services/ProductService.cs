using MagFlow.BLL.Services.Interfaces;
using MagFlow.DAL.Repositories.CompanyScope.Interfaces;
using MagFlow.Domain.CompanyScope;
using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;
using MagFlow.BLL.Mappers.Domain.CompanyScope;
using MagFlow.Shared.Models.FormModels;
using Microsoft.EntityFrameworkCore;

namespace MagFlow.BLL.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductCategoryRepository _categoryRepository;
        private readonly IProductTypeRepository _typeRepository;
        private readonly IProductParameterRepository _parameterRepository;
        private readonly IUnitRepository _unitRepository;

        public ProductService(IProductRepository productRepository,
            IProductCategoryRepository productCategoryRepository,
            IProductTypeRepository productTypeRepository,
            IProductParameterRepository productParameterRepository,
            IUnitRepository unitRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = productCategoryRepository;
            _typeRepository = productTypeRepository;
            _parameterRepository = productParameterRepository;
            _unitRepository = unitRepository;
        }

        public async Task<QueryResponse<ProductDTO>> GetProducts(int pageNumber = 1, int pageSize = 25, string? search = null, string? sortBy = null, bool descending = false)
        {
            var queryResponse = await _productRepository.GetAsync(new QueryOptions<Product>()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Search = search,
                SortBy = sortBy,
                Descending = descending
            });
            return new QueryResponse<ProductDTO>()
            {
                Elements = queryResponse?.Elements.Select(x =>
                {
                    var dto = x.ToDTO();
                    return dto;
                }).ToList() ?? new List<ProductDTO>(),
                TotalCount = queryResponse?.TotalCount ?? 0
            };
        }

        public async Task<QueryResponse<ProductTypeDTO>> GetTypes(int pageNumber = 1, int pageSize = 25, string? search = null, string? sortBy = null, bool descending = false)
        {
            var queryResponse = await _typeRepository.GetAsync(new QueryOptions<ProductType>()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Search = search,
                SortBy = sortBy,
                Descending = descending
            });
            return new QueryResponse<ProductTypeDTO>()
            {
                Elements = queryResponse?.Elements.Select(x =>
                {
                    var dto = x.ToDTO();
                    return dto;
                }).ToList() ?? new List<ProductTypeDTO>(),
                TotalCount = queryResponse?.TotalCount ?? 0
            };
        }

        public async Task<QueryResponse<ProductCategoryDTO>> GetCategories(int pageNumber = 1, int pageSize = 25, string? search = null, string? sortBy = null, bool descending = false)
        {
            var queryResponse = await _categoryRepository.GetAsync(new QueryOptions<ProductCategory>()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Search = search,
                SortBy = sortBy,
                Descending = descending
            });
            return new QueryResponse<ProductCategoryDTO>()
            {
                Elements = queryResponse?.Elements.Select(x =>
                {
                    var dto = x.ToDTO();
                    return dto;
                }).ToList() ?? new List<ProductCategoryDTO>(),
                TotalCount = queryResponse?.TotalCount ?? 0
            };
        }

        public async Task<QueryResponse<ProductParameterDTO>> GetParameters(int pageNumber = 1, int pageSize = 25, string? search = null, string? sortBy = null, bool descending = false)
        {
            var queryResponse = await _parameterRepository.GetAsync(new QueryOptions<ProductParameter>()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Search = search,
                SortBy = sortBy,
                Descending = descending
            });
            return new QueryResponse<ProductParameterDTO>()
            {
                Elements = queryResponse?.Elements.Select(x =>
                {
                    var dto = x.ToDTO();
                    return dto;
                }).ToList() ?? new List<ProductParameterDTO>(),
                TotalCount = queryResponse?.TotalCount ?? 0
            };
        }

        public async Task<QueryResponse<UnitDTO>> GetUnits(int pageNumber = 1, int pageSize = 25, string? search = null, string? sortBy = null, bool descending = false)
        {
            var queryResponse = await _unitRepository.GetAsync(new QueryOptions<Unit>()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Filters = new Dictionary<string, object>()
                {
                    { nameof(Unit.ParentUnitId), null }
                },
                Search = search,
                SortBy = sortBy,
                Descending = descending
            }, x => x.Include(y => y.RelatedUnits));
            return new QueryResponse<UnitDTO>()
            {
                Elements = queryResponse?.Elements.Select(x =>
                {
                    var dto = x.ToDTO();
                    return dto;
                }).ToList() ?? new List<UnitDTO>(),
                TotalCount = queryResponse?.TotalCount ?? 0
            };
        }



        public async Task<Enums.Result> AddProduct(ProductFormModel model)
        {
            return Enums.Result.Error;
        }

        public async Task<Enums.Result> AddType(ProductTypeFormModel model)
        {
            return Enums.Result.Error;
        }

        public async Task<Enums.Result> AddCategory(ProductCategoryFormModel model)
        {
            return Enums.Result.Error;
        }

        public async Task<Enums.Result> AddParameter(ProductParameterFormModel model)
        {
            return Enums.Result.Error;
        }

        public async Task<Enums.Result> AddMeasurementUnit(MeasurementUnitFormModel model)
        {
            return Enums.Result.Error;
        }



        public async Task<Enums.Result> UpdateProduct(ProductDTO productDTO)
        {
            return Enums.Result.Error;
        }

        public async Task<Enums.Result> UpdateType(ProductTypeDTO typeDTO)
        {
            return Enums.Result.Error;
        }

        public async Task<Enums.Result> UpdateCategory(ProductCategoryDTO categoryDTO)
        {
            return Enums.Result.Error;
        }

        public async Task<Enums.Result> UpdateParameter(ProductParameterDTO parameterDTO)
        {
            return Enums.Result.Error;
        }

        public async Task<Enums.Result> UpdateMeasurementUnit(UnitDTO unitDTO)
        {
            return Enums.Result.Error;
        }



        public async Task<Enums.Result> DeleteProduct(ProductDTO productDTO)
        {
            return Enums.Result.Error;
        }

        public async Task<Enums.Result> DeleteProducts(List<ProductDTO> productDTOs)
        {
            return Enums.Result.Error;
        }

        public async Task<Enums.Result> DeleteType(ProductTypeDTO typeDTO)
        {
            return Enums.Result.Error;
        }

        public async Task<Enums.Result> DeleteTypes(List<ProductTypeDTO> typeDTOs)
        {
            return Enums.Result.Error;
        }

        public async Task<Enums.Result> DeleteCategory(ProductCategoryDTO categoryDTO)
        {
            return Enums.Result.Error;
        }

        public async Task<Enums.Result> DeleteCategories(List<ProductCategoryDTO> categoryDTOs)
        {
            return Enums.Result.Error;
        }

        public async Task<Enums.Result> DeleteParameter(ProductParameterDTO parameterDTO)
        {
            return Enums.Result.Error;
        }

        public async Task<Enums.Result> DeleteParameters(List<ProductParameterDTO> parameterDTOs)
        {
            return Enums.Result.Error;
        }

        public async Task<Enums.Result> DeleteMeasurementUnit(UnitDTO unitDTO)
        {
            return Enums.Result.Error;
        }

        public async Task<Enums.Result> DeleteMeasurementUnits(List<UnitDTO> unitDTOs)
        {
            return Enums.Result.Error;
        }
    }
}
