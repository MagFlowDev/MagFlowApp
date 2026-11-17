using FluentValidation;
using MagFlow.BLL.Mappers.Domain.CompanyScope;
using MagFlow.BLL.Services.Interfaces;
using MagFlow.DAL.Repositories.CompanyScope.Interfaces;
using MagFlow.Domain.CompanyScope;
using MagFlow.Shared.DTOs.CompanyScope;
using MagFlow.Shared.Models;
using MagFlow.Shared.Models.FormModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.BLL.Services
{
    public class ContractorService : BaseCompanyService<Contractor, ContractorDTO>, IContractorService
    {
        private readonly IContractorRepository _contractorRepository;

        private readonly INetworkService _networkService;
        private readonly IValidator<ContractorFormModel> _validator;

        public ContractorService(IContractorRepository contractorRepository,
            INetworkService networkService,
            IValidator<ContractorFormModel> validator,
            ILogger<ContractorService> logger) : base(contractorRepository, networkService, logger)
        {
            _contractorRepository = contractorRepository;
            _networkService = networkService;
            _validator = validator;
        }

        public async Task<Enums.Result> AddContractor(ContractorFormModel model)
        {
            var userId = _networkService.GetUserId();
            if (!userId.HasValue || model == null || !(await _validator.ValidateAsync(model)).IsValid)
                return Enums.Result.Error;

            return await _contractorRepository.AddAsync(model.ToEntity(userId.Value));
        }

        public async Task<Enums.Result> ChangeStatuses(IReadOnlyCollection<int> ids, Enums.EntityStatus status)
        {
            if (ids.Count == 0 || ids.Any(id => id <= 0) ||
                status is not (Enums.EntityStatus.Active or Enums.EntityStatus.Inactive))
                return Enums.Result.Error;

            var userId = _networkService.GetUserId();
            if (!userId.HasValue)
                return Enums.Result.Error;

            return await _contractorRepository.ChangeStatuses(ids.Distinct().ToArray(), status, userId.Value);
        }

        public async Task<Enums.Result> RestoreArchivedContractors(IReadOnlyCollection<int> ids)
        {
            if (ids.Count == 0 || ids.Any(id => id <= 0))
                return Enums.Result.Error;

            return await _contractorRepository.RestoreArchivedContractors(ids.Distinct().ToArray());
        }
    }
}
