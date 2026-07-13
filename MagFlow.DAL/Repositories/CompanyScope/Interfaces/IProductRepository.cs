using MagFlow.Domain.CompanyScope;
using MagFlow.EF;
using MagFlow.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.DAL.Repositories.CompanyScope.Interfaces
{
    public interface IProductRepository : IRepository<Product, CompanyDbContext>
    {
        Task<Enums.Result> RemoveProductParameters(List<ProductParameter> parameters);
        Task<Enums.Result> RemoveProductComponents(List<ProductComponent> components);
        Task<Enums.Result> RemoveProductConversions(List<ProductUnitConversion> conversions);

        Task<Enums.Result> UpdateProductConversion(ProductUnitConversion conversion);
    }
}
