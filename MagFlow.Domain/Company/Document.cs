using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MagFlow.Shared.Models;

namespace MagFlow.Domain.Company
{
    public class Document
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string InternalNumber { get; set; }
        public string? ExternalNumber { get; set; }
        public int? OrderId { get; set; }
        public int? WarehouseFromId { get; set; }
        public int? WarehouseToId { get; set; }
        public int? ContractorId { get; set; }
        public int? RelatedDocumentId { get; set; }
        [Required]
        public int DocumentTypeId { get; set; }
        [Required]
        public DateTime DocumentDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        [Required]
        public Enums.DocumentStatus Status { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public Guid CreatedById { get; set; }
        public DateTime? ConfirmedAt { get; set; }
        public Guid? ConfirmedById { get; set; }
        public string? Note { get; set; }
        
        [ForeignKey(nameof(OrderId))]
        public Order? Order { get; set; }
        [ForeignKey(nameof(WarehouseFromId))]
        public Warehouse? WarehouseFrom { get; set; }
        [ForeignKey(nameof(WarehouseToId))]
        public Warehouse? WarehouseTo { get; set; }
        [ForeignKey(nameof(ContractorId))]
        public Contractor? Contractor { get; set; }
        [ForeignKey(nameof(RelatedDocumentId))]
        public Document? RelatedDocument { get; set; }
        [ForeignKey(nameof(DocumentTypeId))]
        public DocumentType? DocumentType { get; set; }
    }
}