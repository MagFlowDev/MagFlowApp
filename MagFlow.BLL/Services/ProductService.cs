using MagFlow.BLL.Services.Interfaces;
using MagFlow.DAL.Repositories.CompanyScope.Interfaces;
using MagFlow.Domain.CompanyScope;
using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;
using MagFlow.BLL.Mappers.Domain.CompanyScope;

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
                Search = search,
                SortBy = sortBy,
                Descending = descending
            });
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
    }
}
