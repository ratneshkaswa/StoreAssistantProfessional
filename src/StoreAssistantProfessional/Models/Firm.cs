using System.ComponentModel.DataAnnotations;

namespace StoreAssistantProfessional.Models;

public class Firm
{
    public const int SingletonId = 1;

    // Single source of truth for system-assigned bill-kind prefixes (Quote /
    // Proforma / DeliveryChallan / CashReceipt). Tax invoices use a fixed
    // date-based format `DD-MMM-YY-NNN` with a daily counter — no user
    // configuration of prefix / padding / start # is exposed.
    public static readonly IReadOnlyDictionary<string, string> BillKindPrefixes =
        new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["Quote"] = "QT",
            ["Proforma"] = "PF",
            ["DeliveryChallan"] = "DC",
            ["CashReceipt"] = "CR",
        };

    public static readonly IReadOnlyCollection<string> ReservedInvoicePrefixes =
        BillKindPrefixes.Values.ToArray();

    [Key]
    public int Id { get; set; } = SingletonId;

    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    // Explicit toggle — not all firms are GST-registered. When false, the
    // form hides the GSTIN field entirely and Normalize() blanks the value
    // so a previously-entered GSTIN doesn't linger as ghost data.
    public bool HasGstRegistration { get; set; }

    [MaxLength(15)] public string? Gstin { get; set; }
    [MaxLength(10)] public string? Pan { get; set; }
    [MaxLength(2)]  public string? StateCode { get; set; }
    [MaxLength(100)] public string? StateName { get; set; }

    public string? LogoBase64 { get; set; }
    [MaxLength(50)] public string? LogoMimeType { get; set; }

    [MaxLength(15)]  public string? Phone { get; set; }
    [MaxLength(200)] public string? Email { get; set; }

    [MaxLength(300)] public string? AddressLine1 { get; set; }
    [MaxLength(100)] public string? City { get; set; }
    [MaxLength(6)]   public string? Pincode { get; set; }

    [MaxLength(200)] public string? BankName { get; set; }
    [MaxLength(100)] public string? AccountHolderName { get; set; }
    [MaxLength(20)]  public string? AccountNumber { get; set; }
    [MaxLength(11)]  public string? IfscCode { get; set; }
    [MaxLength(200)] public string? BranchName { get; set; }
    [MaxLength(100)] public string? UpiId { get; set; }

    [Range(1, 31)] public int FyStartDay { get; set; } = 1;
    [Range(1, 12)] public int FyStartMonth { get; set; } = 4;

    // Set on the Tax onboarding step (not here) — composition scheme is a
    // tax-classification choice, not a firm-identity field. Keeping the column
    // on Firm because it's a singleton firm-level boolean; just the editing
    // surface lives elsewhere.
    public bool IsCompositionScheme { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
