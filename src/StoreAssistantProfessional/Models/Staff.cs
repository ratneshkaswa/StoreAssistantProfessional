using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreAssistantProfessional.Models;

public class Staff
{
    [Key] public int Id { get; set; }

    [Range(1, 9999)] public int Code { get; set; }

    [Required, MaxLength(200)] public string Name { get; set; } = string.Empty;
    [MaxLength(15)]  public string? Phone { get; set; }
    [MaxLength(200)] public string? Email { get; set; }
    [MaxLength(50)]  public string? Role { get; set; }

    public string? PhotoBase64 { get; set; }
    [MaxLength(50)] public string? PhotoMimeType { get; set; }

    [MaxLength(20)]  public string? IdProofNumber { get; set; }
    [MaxLength(50)]  public string? IdProofType { get; set; }

    [MaxLength(300)] public string? Address { get; set; }
    [MaxLength(100)] public string? City { get; set; }
    [MaxLength(6)]   public string? Pincode { get; set; }

    [Range(0, 100)]
    [Column(TypeName = "decimal(5,2)")]
    public decimal CommissionPercent { get; set; }

    [Range(0, 9999999)]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Salary { get; set; }

    [MaxLength(200)] public string? SalaryFormula { get; set; }

    public DateTime? JoiningDate { get; set; }

    [MaxLength(500)] public string? Notes { get; set; }

    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
