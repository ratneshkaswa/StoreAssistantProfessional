using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreAssistantProfessional.Models;

public class InwardEntry
{
    [Key] public int Id { get; set; }

    [Required, MaxLength(20)] public string InwardNumber { get; set; } = string.Empty;

    public DateTime EntryDate { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "decimal(18,2)")] public decimal TransportCharges { get; set; }

    [MaxLength(100)] public string? Carrier { get; set; }
    [MaxLength(50)]  public string? LrNumber { get; set; }
    [MaxLength(20)]  public string? VehicleNumber { get; set; }
    [MaxLength(100)] public string? DriverInfo { get; set; }

    [MaxLength(500)] public string? Notes { get; set; }

    [MaxLength(20)] public string Status { get; set; } = "Draft";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ReceivedAt { get; set; }

    public List<InwardParcel> Parcels { get; set; } = new();
}

public class InwardParcel
{
    [Key] public int Id { get; set; }

    public int InwardEntryId { get; set; }
    [ForeignKey(nameof(InwardEntryId))] public InwardEntry? InwardEntry { get; set; }

    public int VendorId { get; set; }
    [ForeignKey(nameof(VendorId))] public Vendor? Vendor { get; set; }

    [MaxLength(50)] public string? VendorInvoiceNumber { get; set; }
    public DateTime? VendorInvoiceDate { get; set; }

    [MaxLength(200)] public string? Notes { get; set; }

    public List<InwardParcelItem> Items { get; set; } = new();
}

public class InwardParcelItem
{
    [Key] public int Id { get; set; }

    public int InwardParcelId { get; set; }
    [ForeignKey(nameof(InwardParcelId))] public InwardParcel? InwardParcel { get; set; }

    public int ProductId { get; set; }
    [ForeignKey(nameof(ProductId))] public Product? Product { get; set; }

    [Column(TypeName = "decimal(18,3)")] public decimal Quantity { get; set; }
    [Column(TypeName = "decimal(18,3)")] public decimal DamagedQuantity { get; set; }

    [Column(TypeName = "decimal(18,2)")] public decimal UnitCost { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal LineTotal { get; set; }
}
