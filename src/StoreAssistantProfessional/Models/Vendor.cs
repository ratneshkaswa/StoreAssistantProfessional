using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreAssistantProfessional.Models;

public class Vendor
{
    [Key] public int Id { get; set; }

    [Required, MaxLength(200)] public string Name { get; set; } = string.Empty;
    [MaxLength(15)]  public string? Phone { get; set; }
    [MaxLength(15)]  public string? AltPhone { get; set; }
    [MaxLength(200)] public string? Email { get; set; }
    [MaxLength(15)]  public string? Gstin { get; set; }

    [MaxLength(300)] public string? Address { get; set; }
    [MaxLength(100)] public string? City { get; set; }
    [MaxLength(100)] public string? State { get; set; }
    [MaxLength(6)]   public string? Pincode { get; set; }

    [MaxLength(200)] public string? BankName { get; set; }
    [MaxLength(100)] public string? AccountHolderName { get; set; }
    [MaxLength(20)]  public string? AccountNumber { get; set; }
    [MaxLength(11)]  public string? IfscCode { get; set; }

    [Column(TypeName = "decimal(18,2)")] public decimal Balance { get; set; }
    public int PaymentTermDays { get; set; } = 30;
    [Column(TypeName = "decimal(18,2)")] public decimal CreditLimit { get; set; }

    [MaxLength(500)] public string? Notes { get; set; }

    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
