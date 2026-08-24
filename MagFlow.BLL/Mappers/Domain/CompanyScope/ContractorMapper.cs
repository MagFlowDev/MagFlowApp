using MagFlow.Domain.CompanyScope;
using MagFlow.Shared.DTOs.CompanyScope;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace MagFlow.BLL.Mappers.Domain.CompanyScope
{
    public static class ContractorMapper
    {
        public static ContractorDTO ToDTO(this Contractor contractor)
        {
            if (contractor == null)
                return null;
            return new ContractorDTO
            {
                Id = contractor.Id,
                Name = contractor.Name,
                Code = contractor.Code,
                Note = contractor.Note,

                TaxNumber = contractor.TaxNumber,
                Address = contractor.Address,
                PostalCode = contractor.PostalCode,
                City = contractor.City,
                Country = contractor.Country,

                ContactEmail = contractor.ContactEmail,
                ContactPerson = contractor.ContactPerson,
                ContactPhone = contractor.ContactPhone,

                RemovedAt = contractor.RemovedAt,
                Status = contractor.Status,
            };
        }

        public static List<ContractorDTO> ToDTO(this IEnumerable<Contractor> contractors)
        {
            return contractors.Select(x => x.ToDTO()).ToList();
        }



        public static Contractor ToEntity(this ContractorDTO contractorDTO)
        {
            if (contractorDTO == null)
                return null;
            return new Contractor
            {
                Id = contractorDTO.Id,
                Name = contractorDTO.Name,
                Note = contractorDTO.Note,

                TaxNumber = contractorDTO.TaxNumber,
                Address = contractorDTO.Address,
                PostalCode = contractorDTO.PostalCode,
                City = contractorDTO.City,
                Country = contractorDTO.Country,

                ContactEmail = contractorDTO.ContactEmail,
                ContactPerson = contractorDTO.ContactPerson,
                ContactPhone = contractorDTO.ContactPhone,

                RemovedAt = contractorDTO.RemovedAt,
                Status = contractorDTO.Status,
            };
        }

        public static List<Contractor> ToEntity(this IEnumerable<ContractorDTO> contractorsDTOs)
        {
            return contractorsDTOs.Select(x => x.ToEntity()).ToList();
        }

    }
}
