using MagFlow.BLL.Mappers.Domain.CompanyScope;
using MagFlow.BLL.Services.Interfaces;
using MagFlow.DAL.Repositories.CompanyScope.Interfaces;
using MagFlow.Domain.CompanyScope;
using MagFlow.Domain.CoreScope;
using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.DTOs.CoreScope;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.FormModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace MagFlow.BLL.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductCategoryRepository _categoryRepository;
        private readonly IProductTypeRepository _typeRepository;
        private readonly IParameterRepository _parameterRepository;
        private readonly IProductParameterRepository _productParameterRepository;
        private readonly IUnitRepository _unitRepository;

        private readonly INetworkService _networkService;

        public ProductService(IProductRepository productRepository,
            IProductCategoryRepository productCategoryRepository,
            IProductTypeRepository productTypeRepository,
            IParameterRepository parameterRepository,
            IProductParameterRepository productParameterRepository,
            IUnitRepository unitRepository,
            INetworkService networkService)
        {
            _productRepository = productRepository;
            _categoryRepository = productCategoryRepository;
            _typeRepository = productTypeRepository;
            _parameterRepository = parameterRepository;
            _productParameterRepository = productParameterRepository;
            _unitRepository = unitRepository;
            _networkService = networkService;
        }

        public async Task<ProductDTO?> GetProduct(int id)
        {
            var product = await _productRepository.GetByIdAsync(id, product => product
                .Include(x => x.Category)
                .Include(x => x.Type)
                .Include(x => x.Unit)
                .Include(x => x.Parameters));
            var dto = product?.ToDTO();
            return dto;
        }

        public async Task<QueryResponse<ProductDTO>> GetProducts(int pageNumber = 0, int pageSize = 25, string? search = null, string? sortBy = null, bool descending = false)
        {
            var queryResponse = await _productRepository.GetAsync(new QueryOptions<Product>()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Search = search,
                SearchColumns = new Expression<Func<Product, string?>>[]
                {
                    u => u.Name
                },
                SortBy = sortBy,
                Descending = descending
            }, products => products
                .Include(x => x.Category)
                .Include(x => x.Type).ThenInclude(y => y.Category)
                .Include(x => x.Unit).ThenInclude(y => y.RelatedUnits)
                .Include(x => x.Parameters).ThenInclude(y => y.Parameter).ThenInclude(z => z.Unit));
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

        public async Task<QueryResponse<ProductTypeDTO>> GetTypes(int pageNumber = 0, int pageSize = 25, string? search = null, string? sortBy = null, bool descending = false, ProductCategoryDTO? productCategory = null)
        {
            var queryOptions = new QueryOptions<ProductType>()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Search = search,
                SearchColumns = new Expression<Func<ProductType, string?>>[]
                {
                    u => u.Name
                },
                SortBy = sortBy,
                Descending = descending
            };
            if (productCategory != null)
            {
                queryOptions.Filters = new Dictionary<string, object>()
                {
                    { nameof(ProductType.CategoryId), productCategory.Id }
                };
            }
            var queryResponse = await _typeRepository.GetAsync(queryOptions, type => type.Include(x => x.Category));
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

        public async Task<QueryResponse<ProductCategoryDTO>> GetCategories(int pageNumber = 0, int pageSize = 25, string? search = null, string? sortBy = null, bool descending = false)
        {
            var queryResponse = await _categoryRepository.GetAsync(new QueryOptions<ProductCategory>()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Search = search,
                SearchColumns = new Expression<Func<ProductCategory, string?>>[]
                {
                    u => u.Name
                },
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

        public async Task<QueryResponse<ParameterDTO>> GetParameters(int pageNumber = 0, int pageSize = 25, string? search = null, string? sortBy = null, bool descending = false)
        {
            var queryResponse = await _parameterRepository.GetAsync(new QueryOptions<CustomParameter>()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Search = search,
                SearchColumns = new Expression<Func<CustomParameter, string?>>[]
                {
                    u => u.Name
                },
                SortBy = sortBy,
                Descending = descending
            }, parameters => parameters.Include(x => x.Unit));
            return new QueryResponse<ParameterDTO>()
            {
                Elements = queryResponse?.Elements.Select(x =>
                {
                    var dto = x.ToDTO();
                    return dto;
                }).ToList() ?? new List<ParameterDTO>(),
                TotalCount = queryResponse?.TotalCount ?? 0
            };
        }

        public async Task<QueryResponse<UnitDTO>> GetUnits(int pageNumber = 0, int pageSize = 25, string? search = null, string? sortBy = null, bool descending = false)
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
                SearchColumns = new Expression<Func<Unit, string?>>[]
                {
                    x => x.Name
                },
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
            var userId = _networkService.GetUserId();
            if (!userId.HasValue)
                return Enums.Result.Error;
            var unit = model.ToEntity(userId.Value);
            var result = await _productRepository.AddAsync(unit);
            return result;
        }

        public async Task<Enums.Result> AddType(ProductTypeFormModel model)
        {
            var unit = model.ToEntity();
            var result = await _typeRepository.AddAsync(unit);
            return result;
        }

        public async Task<Enums.Result> AddCategory(ProductCategoryFormModel model)
        {
            var unit = model.ToEntity();
            var result = await _categoryRepository.AddAsync(unit);
            return result;
        }

        public async Task<Enums.Result> AddParameter(ParameterFormModel model)
        {
            var unit = model.ToEntity();
            var result = await _parameterRepository.AddAsync(unit);
            return result;
        }

        public async Task<Enums.Result> AddMeasurementUnit(MeasurementUnitFormModel model)
        {
            var unit = model.ToEntity();
            var result = await _unitRepository.AddAsync(unit);
            return result;
        }



        public async Task<Enums.Result> UpdateProduct(ProductDTO productDTO)
        {
            var product = productDTO.ToEntity();
            var result = await _productRepository.UpdateAsync(product);
            return result;
        }

        public async Task<Enums.Result> UpdateType(ProductTypeDTO typeDTO)
        {
            var type = typeDTO.ToEntity();
            var result = await _typeRepository.UpdateAsync(type);
            return result;
        }

        public async Task<Enums.Result> UpdateCategory(ProductCategoryDTO categoryDTO)
        {
            var category = categoryDTO.ToEntity();
            var result = await _categoryRepository.UpdateAsync(category);
            return result;
        }

        public async Task<Enums.Result> UpdateParameter(ParameterDTO parameterDTO)
        {
            var parameter = parameterDTO.ToEntity();
            var result = await _parameterRepository.UpdateAsync(parameter);
            return result;
        }

        public async Task<Enums.Result> UpdateMeasurementUnit(UnitDTO unitDTO, List<int>? removedUnits = null)
        {
            var unit = unitDTO.ToEntity();
            var originalUnit = await _unitRepository.GetByIdAsync(unit.Id, unit => unit.Include(x => x.RelatedUnits));
            if (removedUnits != null && removedUnits.Any())
            {
                foreach (var removedUnitId in removedUnits)
                {
                    var toRemove = originalUnit?.RelatedUnits.FirstOrDefault(x => x.Id == removedUnitId);
                    if(toRemove == null)
                        continue;
                    var tempResult = await _unitRepository.DeleteAsync(toRemove);
                    if (tempResult != Enums.Result.Success)
                        return tempResult;
                }
            }

            var result = await _unitRepository.UpdateAsync(unit);
            return result;
        }



        public async Task<Enums.Result> DeleteProduct(ProductDTO productDTO)
        {
            var originalProduct = await _productRepository.GetByIdAsync(productDTO.Id);
            if (originalProduct == null)
                return Enums.Result.Error;

            var result = await _productRepository.DeleteAsync(originalProduct);
            return result;
        }

        public async Task<Enums.Result> DeleteProducts(List<ProductDTO> productDTOs)
        {
            var productsIds = productDTOs.Select(x => x.Id).ToList();
            var result = await _productRepository.DeleteManyAsync(x => productsIds.Contains(x.Id));
            return result;
        }

        public async Task<Enums.Result> DeleteType(ProductTypeDTO typeDTO)
        {
            var originalType = await _typeRepository.GetByIdAsync(typeDTO.Id);
            if (originalType == null)
                return Enums.Result.Error;

            var result = await _typeRepository.DeleteAsync(originalType);
            return result;
        }

        public async Task<Enums.Result> DeleteTypes(List<ProductTypeDTO> typeDTOs)
        {
            var typesIds = typeDTOs.Select(x => x.Id).ToList();
            var result = await _typeRepository.DeleteManyAsync(x => typesIds.Contains(x.Id));
            return result;
        }

        public async Task<Enums.Result> DeleteCategory(ProductCategoryDTO categoryDTO)
        {
            var originalCategory = await _categoryRepository.GetByIdAsync(categoryDTO.Id);
            if (originalCategory == null)
                return Enums.Result.Error;

            var result = await _categoryRepository.DeleteAsync(originalCategory);
            return result;
        }

        public async Task<Enums.Result> DeleteCategories(List<ProductCategoryDTO> categoryDTOs)
        {
            var categoriesIds = categoryDTOs.Select(x => x.Id).ToList();
            var result = await _categoryRepository.DeleteManyAsync(x => categoriesIds.Contains(x.Id));
            return result;
        }

        public async Task<Enums.Result> DeleteParameter(ParameterDTO parameterDTO)
        {
            var originalParameter = await _parameterRepository.GetByIdAsync(parameterDTO.Id);
            if (originalParameter == null)
                return Enums.Result.Error;

            var result = await _parameterRepository.DeleteAsync(originalParameter);
            return result;
        }

        public async Task<Enums.Result> DeleteParameters(List<ParameterDTO> parameterDTOs)
        {
            var parametersIds = parameterDTOs.Select(x => x.Id).ToList();
            var result = await _parameterRepository.DeleteManyAsync(x => parametersIds.Contains(x.Id));
            return result;
        }

        public async Task<Enums.Result> DeleteMeasurementUnit(UnitDTO unitDTO)
        {
            var originalUnit = await _unitRepository.GetByIdAsync(unitDTO.Id);
            if (originalUnit == null)
                return Enums.Result.Error;

            var result = await _unitRepository.DeleteAsync(originalUnit);
            return result;
        }

        public async Task<Enums.Result> DeleteMeasurementUnits(List<UnitDTO> unitDTOs)
        {
            var unitsIds = unitDTOs.Select(x => x.Id);
            var originalUnits = (await _unitRepository.GetAllAsync(unit => unitsIds.Contains(unit.Id), unit => unit.Include(x => x.RelatedUnits))).ToList();

            var now = DateTime.UtcNow;
            originalUnits.ForEach(unit =>
            {
                foreach (var child in unit.RelatedUnits)
                    child.RemovedAt = now;
            });
            
            var result = await _unitRepository.DeleteManyAsync(originalUnits);
            return result;
        }
    }
}
