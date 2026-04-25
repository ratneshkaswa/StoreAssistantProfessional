using System.ComponentModel.DataAnnotations;

namespace StoreAssistantProfessional.Models;

public class Firm
{
    public const int SingletonId = 1;

    [Key]
    public int Id { get; set; } = SingletonId;

    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(15)] public string? Gstin { get; set; }
    [MaxLength(10)] public string? Pan { get; set; }
    [MaxLength(2)]  public string? StateCode { get; set; }
    [MaxLength(100)] public string? StateName { get; set; }

    public string? LogoBase64 { get; set; }
    [MaxLength(50)] public string? LogoMimeType { get; set; }

    [MaxLength(15)]  public string? Phone { get; set; }
    [MaxLength(15)]  public string? AltPhone { get; set; }
    [MaxLength(200)] public string? Email { get; set; }

    [MaxLength(300)] public string? AddressLine1 { get; set; }
    [MaxLength(300)] public string? AddressLine2 { get; set; }
    [MaxLength(100)] public string? City { get; set; }
    [MaxLength(6)]   public string? Pincode { get; set; }

    [MaxLength(200)] public string? BankName { get; set; }
    [MaxLength(100)] public string? AccountHolderName { get; set; }
    [MaxLength(20)]  public string? AccountNumber { get; set; }
    [MaxLength(11)]  public string? IfscCode { get; set; }
    [MaxLength(200)] public string? BranchName { get; set; }
    [MaxLength(100)] public string? UpiId { get; set; }

    [MaxLength(20)] public string InvoicePrefix { get; set; } = "INV";
    public int InvoiceStartNumber { get; set; } = 1;
    public int NextInvoiceNumber { get; set; } = 1;
    [MaxLength(10)] public string InvoiceNumberPadding { get; set; } = "0000";
    public bool ResetInvoiceOnFyRollover { get; set; } = true;

    [MaxLength(20)] public string DateFormat { get; set; } = "dd/MM/yyyy";

    [Range(1, 31)] public int FyStartDay { get; set; } = 1;
    [Range(1, 12)] public int FyStartMonth { get; set; } = 4;

    public bool IsCompositionScheme { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
