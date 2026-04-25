using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreAssistantProfessional.Models;

public class StockAdjustment
{
    [Key] public int Id { get; set; }

    public int ProductId { get; set; }
    [ForeignKey(nameof(ProductId))] public Product? Product { get; set; }

    public DateTime At { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "decimal(18,3)")] public decimal SystemQty { get; set; }
    [Column(TypeName = "decimal(18,3)")] public decimal CountedQty { get; set; }
    [Column(TypeName = "decimal(18,3)")] public decimal Diff { get; set; }

    [MaxLength(50)]  public string? Kind { get; set; }     // StockTake, Damage, Theft, Correction
    [MaxLength(200)] public string? Reason { get; set; }

    [MaxLength(50)] public string? BatchTag { get; set; } // groups one stock-take run

    [MaxLength(20)] public string? OperatorRole { get; set; }   // User / Admin
}
