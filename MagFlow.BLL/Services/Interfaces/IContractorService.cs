using MagFlow.Domain.CompanyScope;
using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.FormModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.BLL.Services.Interfaces
{
    public interface IContractorService : IBaseCompanyService<Contractor, ContractorDTO>
    {
        Task<Enums.Result> AddContractor(ContractorFormModel model);
        Task<Enums.Result> ChangeStatuses(IReadOnlyCollection<int> ids, Enums.EntityStatus status);
        Task<Enums.Result> RestoreArchivedContractors(IReadOnlyCollection<int> ids);
    }
}
