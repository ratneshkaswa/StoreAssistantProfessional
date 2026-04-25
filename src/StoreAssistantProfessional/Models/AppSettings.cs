using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreAssistantProfessional.Models;

public class AppSettings
{
    public const int SingletonId = 1;

    [Key] public int Id { get; set; } = SingletonId;

    // Cash management
    [MaxLength(20)] public string CashMode { get; set; } = "SingleDrawer";
    [Column(TypeName = "decimal(18,2)")] public decimal PettyBoxFloat { get; set; } = 1000m;
    [Column(TypeName = "decimal(18,2)")] public decimal PettyBoxRefillPrompt { get; set; } = 300m;
    [Column(TypeName = "decimal(18,2)")] public decimal PettyBoxRefillStep { get; set; } = 500m;

    // Goals
    [Column(TypeName = "decimal(18,2)")] public decimal DailyGoal { get; set; } = 50_000m;
    [Column(TypeName = "decimal(18,2)")] public decimal MonthlyGoal { get; set; } = 12_00_000m;
    public bool ShowGoalBar { get; set; } = true;
    public bool ShowStreak { get; set; } = true;
    public bool ShowComparisons { get; set; } = true;
    public bool GoalNudgeEnabled { get; set; } = true;
    public bool ShowEomWrapUp { get; set; } = true;
    [MaxLength(20)] public string? EomDismissedFor { get; set; } // YYYY-MM of the wrap-up the user dismissed

    // Inventory
    [MaxLength(20)] public string InventoryValuationMethod { get; set; } = "LastPurchase"; // LastPurchase / Average

    // Permissions — User role can do these without unlocking Admin
    public bool UserCanGiveDiscount { get; set; } = true;
    public bool UserCanRefund { get; set; }
    public bool UserCanVoidBill { get; set; }
    public bool UserCanEditSavedBill { get; set; }
    public bool UserCanAdjustStock { get; set; }
    public bool UserCanEditSettings { get; set; }
    public bool UserCanOpenDrawer { get; set; } = true;
    public bool UserCanRunReports { get; set; } = true;
    public bool UserCanExport { get; set; }
    public bool UserCanReset { get; set; }

    // E-way / E-invoice (NIC API)
    public bool EwayEnabled { get; set; }
    [MaxLength(60)]  public string? EwayUserName { get; set; }
    [MaxLength(60)]  public string? EwayPassword { get; set; }
    [MaxLength(15)]  public string? EwayGstin { get; set; }
    public bool EinvoiceEnabled { get; set; }
    [MaxLength(60)]  public string? EinvoiceClientId { get; set; }
    [MaxLength(60)]  public string? EinvoiceClientSecret { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal EinvoiceTurnoverThreshold { get; set; } = 5_00_00_000m;

    // Receipt locale
    [MaxLength(10)] public string ReceiptLanguage { get; set; } = "en"; // en / hi
    public bool ReceiptBilingual { get; set; }

    // Owner alerts
    [MaxLength(15)]  public string? OwnerPhone { get; set; }
    [MaxLength(20)]  public string AlertChannel { get; set; } = "WhatsApp";
    [Range(0, 23)]   public int AlertHour { get; set; } = 21;
    [Range(0, 59)]   public int AlertMinute { get; set; } = 30;
    public bool DailySummaryEnabled { get; set; }

    // Anomaly thresholds
    public bool AnomalyRefundQuick { get; set; } = true;
    public int AnomalyRefundQuickMinutes { get; set; } = 5;
    public bool AnomalyDiscountHigh { get; set; } = true;
    public int AnomalyDiscountPct { get; set; } = 25;
    public bool AnomalyVoidAfterCheckout { get; set; } = true;
    public bool AnomalyDrawerMismatch { get; set; } = true;
    [Column(TypeName = "decimal(18,2)")] public decimal AnomalyDrawerMismatchAmount { get; set; } = 100m;
    public bool AnomalyOffHours { get; set; } = true;
    [Range(0, 23)] public int AnomalyOffHoursAfter { get; set; } = 22;
    [Range(0, 23)] public int AnomalyOffHoursBefore { get; set; } = 9;
    public bool AnomalyRepeatRefunds { get; set; } = true;
    public int AnomalyRepeatRefundsCount { get; set; } = 3;
    public int AnomalyRepeatRefundsDays { get; set; } = 7;

    // Update channel
    [MaxLength(10)] public string UpdateChannel { get; set; } = "Stable";
    [MaxLength(20)] public string UpdateCheckFrequency { get; set; } = "Weekly";
    [MaxLength(20)] public string UpdateInstallMode { get; set; } = "OnNextExit";
    public DateTime? LastUpdateCheck { get; set; }

    // Cloud backup
    public bool BackupLocal { get; set; } = true;
    public bool BackupGoogleDrive { get; set; }
    public bool BackupOneDrive { get; set; }
    public bool BackupEmail { get; set; }
    [MaxLength(200)] public string? BackupEmailAddress { get; set; }
    [MaxLength(20)] public string BackupSchedule { get; set; } = "Daily";
    [Range(0, 23)] public int BackupHour { get; set; } = 21;
    [Range(0, 59)] public int BackupMinute { get; set; } = 30;
    public int BackupRetentionCount { get; set; } = 30;

    // Operations
    public bool PowerUserMode { get; set; }
    public int AdminAutoDropMinutes { get; set; } = 5;
    public int IdleScreenMinutes { get; set; } = 10;
    public bool CrashRecoveryEnabled { get; set; } = true;

    // Receipt template
    [MaxLength(20)] public string ReceiptPaper { get; set; } = "3in";   // 3in / 4in / A5 / A4
    public bool ReceiptShowLogo { get; set; } = true;
    public bool ReceiptShowAddress { get; set; } = true;
    public bool ReceiptShowGstin { get; set; } = true;
    public bool ReceiptShowBillNumber { get; set; } = true;
    public bool ReceiptShowCashier { get; set; }
    public bool ReceiptShowCustomer { get; set; } = true;
    public bool ReceiptShowItems { get; set; } = true;
    public bool ReceiptShowTaxSplit { get; set; } = true;
    public bool ReceiptShowTender { get; set; } = true;
    public bool ReceiptShowQrUpi { get; set; } = true;
    public bool ReceiptShowFooter { get; set; } = true;
    [MaxLength(500)] public string ReceiptFooterText { get; set; } = "Thank you for shopping with us!";

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
