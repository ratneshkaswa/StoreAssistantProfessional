using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreAssistantProfessional.Models;

public class Product
{
    [Key] public int Id { get; set; }

    [Required, MaxLength(200)] public string Name { get; set; } = string.Empty;

    [MaxLength(50)]  public string? Sku { get; set; }
    [MaxLength(100)] public string? Barcode { get; set; }
    [MaxLength(100)] public string? Category { get; set; }
    [MaxLength(8)]   public string? HsnCode { get; set; }

    public int? TaxRateId { get; set; }
    [ForeignKey(nameof(TaxRateId))] public TaxRate? TaxRate { get; set; }

    [Column(TypeName = "decimal(18,2)")] public decimal Mrp { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal SalePrice { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal Cost { get; set; }

    [Column(TypeName = "decimal(18,3)")] public decimal StockQty { get; set; }
    [Column(TypeName = "decimal(18,3)")] public decimal? ReorderLevel { get; set; }

    [MaxLength(500)] public string? Notes { get; set; }

    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
