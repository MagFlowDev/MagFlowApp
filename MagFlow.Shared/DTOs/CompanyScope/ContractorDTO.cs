using MagFlow.Shared.Models;
using MagFlow.Shared.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.Shared.DTOs.CompanyScope
{
    public class ContractorDTO : IBaseDTO, ICodeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; init; }
        public string? TaxNumber { get; set; }
        public string? Address { get; set; }
        public string? PostalCode { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? ContactPhone { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactPerson { get; set; }
        public string? Note { get; set; }
        public DateTime? RemovedAt { get; set; }
        public Enums.EntityStatus Status { get; set; }
    }
}
