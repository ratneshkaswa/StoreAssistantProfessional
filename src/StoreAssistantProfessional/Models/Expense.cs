using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreAssistantProfessional.Models;

public class Expense
{
    [Key] public int Id { get; set; }

    public DateTime At { get; set; } = DateTime.UtcNow;

    [MaxLength(100)] public string Category { get; set; } = "Petty";  // Rent / Utility / Petty / Vendor / Salary / Other
    [MaxLength(200)] public string? Description { get; set; }

    [Column(TypeName = "decimal(18,2)")] public decimal Amount { get; set; }

    [MaxLength(20)] public string Source { get; set; } = "Drawer"; // Drawer / PettyBox

    [MaxLength(100)] public string? Reference { get; set; } // cheque #, vendor invoice #, etc.

    public int? RecurringExpenseId { get; set; }
    [ForeignKey(nameof(RecurringExpenseId))] public RecurringExpense? Recurring { get; set; }
}

public class RecurringExpense
{
    [Key] public int Id { get; set; }

    [Required, MaxLength(200)] public string Name { get; set; } = string.Empty;
    [MaxLength(100)] public string Category { get; set; } = "Other";

    [Column(TypeName = "decimal(18,2)")] public decimal Amount { get; set; }

    [MaxLength(20)] public string Frequency { get; set; } = "Monthly"; // Monthly / Yearly
    [Range(1, 31)] public int DayOfMonth { get; set; } = 1;
    [Range(1, 12)] public int? Month { get; set; }

    public DateTime? NextDue { get; set; }
    public DateTime? LastPosted { get; set; }

    public bool AutoPost { get; set; } = true;
    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class Cheque
{
    [Key] public int Id { get; set; }

    [Required, MaxLength(20)] public string Direction { get; set; } = "Issued"; // Issued / Received

    [Required, MaxLength(50)] public string Number { get; set; } = string.Empty;
    public DateTime IssuedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ClearedAt { get; set; }
    public DateTime? BouncedAt { get; set; }

    [MaxLength(200)] public string Party { get; set; } = string.Empty;
    [MaxLength(200)] public string? BankName { get; set; }

    [Column(TypeName = "decimal(18,2)")] public decimal Amount { get; set; }

    [MaxLength(20)] public string Status { get; set; } = "Pending"; // Pending / Cleared / Bounced

    public int? VendorId { get; set; }
    [ForeignKey(nameof(VendorId))] public Vendor? Vendor { get; set; }

    public int? CustomerId { get; set; }
    [ForeignKey(nameof(CustomerId))] public Customer? Customer { get; set; }

    [MaxLength(500)] public string? Notes { get; set; }
}
