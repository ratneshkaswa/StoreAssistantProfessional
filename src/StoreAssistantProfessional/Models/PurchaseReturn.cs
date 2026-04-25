using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreAssistantProfessional.Models;

public class PurchaseReturn
{
    [Key] public int Id { get; set; }

    [Required, MaxLength(20)] public string ReturnNumber { get; set; } = string.Empty;

    public DateTime At { get; set; } = DateTime.UtcNow;

    public int VendorId { get; set; }
    [ForeignKey(nameof(VendorId))] public Vendor? Vendor { get; set; }

    public int? SourceInwardEntryId { get; set; }
    [ForeignKey(nameof(SourceInwardEntryId))] public InwardEntry? SourceInwardEntry { get; set; }

    [Column(TypeName = "decimal(18,2)")] public decimal Total { get; set; }

    [MaxLength(500)] public string? Notes { get; set; }
    [MaxLength(20)]  public string Status { get; set; } = "Posted"; // Draft / Posted

    public List<PurchaseReturnItem> Items { get; set; } = new();
}

public class PurchaseReturnItem
{
    [Key] public int Id { get; set; }

    public int PurchaseReturnId { get; set; }
    [ForeignKey(nameof(PurchaseReturnId))] public PurchaseReturn? PurchaseReturn { get; set; }

    public int ProductId { get; set; }
    [ForeignKey(nameof(ProductId))] public Product? Product { get; set; }

    [Column(TypeName = "decimal(18,3)")] public decimal Quantity { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal UnitCost { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal LineTotal { get; set; }

    [MaxLength(100)] public string? Reason { get; set; } // Defective / Wrong-item / Excess / Other
}
