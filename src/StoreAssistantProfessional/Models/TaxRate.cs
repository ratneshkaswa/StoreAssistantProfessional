using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreAssistantProfessional.Models;

public class TaxRate
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Column(TypeName = "decimal(5,2)")]
    public decimal Rate { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal CessRate { get; set; }

    [MaxLength(30)] public string? CessType { get; set; }

    public bool IsTaxInclusive { get; set; }

    [Column(TypeName = "decimal(18,2)")] public decimal? MinPrice { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal? MaxPrice { get; set; }

    [MaxLength(8)] public string? HsnCode { get; set; }

    [MaxLength(20)] public string TaxCategory { get; set; } = "Taxable";

    public bool IsDefault { get; set; }

    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [NotMapped] public bool HasSlab => MinPrice.HasValue;
    [NotMapped] public decimal CgstRate => Rate / 2;
    [NotMapped] public decimal SgstRate => Rate / 2;

    [NotMapped]
    public string SlabDisplay => HasSlab
        ? (MaxPrice.HasValue
            ? $"₹{MinPrice:N0} – ₹{MaxPrice:N0}"
            : $"₹{MinPrice:N0} & above")
        : "—";
}
