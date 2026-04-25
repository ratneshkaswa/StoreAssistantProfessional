using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreAssistantProfessional.Models;

public class Bill
{
    [Key] public int Id { get; set; }

    [Required, MaxLength(30)] public string Number { get; set; } = string.Empty;
    public DateTime At { get; set; } = DateTime.UtcNow;

    [MaxLength(20)] public string Status { get; set; } = "Open"; // Open / Held / Posted / Voided
    [MaxLength(20)] public string Kind { get; set; } = "TaxInvoice"; // TaxInvoice / CashReceipt / Quote / Proforma / DeliveryChallan
    public bool IsComposition { get; set; }

    public int? CustomerId { get; set; }
    [ForeignKey(nameof(CustomerId))] public Customer? Customer { get; set; }

    public int? StaffId { get; set; }
    [ForeignKey(nameof(StaffId))] public Staff? Staff { get; set; }

    [Column(TypeName = "decimal(18,2)")] public decimal Subtotal { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal DiscountTotal { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal TaxTotal { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal Total { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal TenderedTotal { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal ChangeAmount { get; set; }

    [MaxLength(500)] public string? Notes { get; set; }
    [MaxLength(200)] public string? VoidReason { get; set; }

    public DateTime? PostedAt { get; set; }
    public DateTime? VoidedAt { get; set; }

    public List<BillItem> Items { get; set; } = new();
    public List<BillPayment> Payments { get; set; } = new();
}

public class BillItem
{
    [Key] public int Id { get; set; }

    public int BillId { get; set; }
    [ForeignKey(nameof(BillId))] public Bill? Bill { get; set; }

    public int ProductId { get; set; }
    [ForeignKey(nameof(ProductId))] public Product? Product { get; set; }

    [MaxLength(200)] public string Description { get; set; } = string.Empty;
    [MaxLength(50)]  public string? Sku { get; set; }
    [MaxLength(100)] public string? Barcode { get; set; }
    [MaxLength(8)]   public string? HsnCode { get; set; }

    [Column(TypeName = "decimal(18,3)")] public decimal Quantity { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal UnitPrice { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal Discount { get; set; }

    public int? TaxRateId { get; set; }
    [ForeignKey(nameof(TaxRateId))] public TaxRate? TaxRate { get; set; }

    [Column(TypeName = "decimal(5,2)")]  public decimal TaxPercent { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal TaxAmount { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal LineTotal { get; set; }
}

public class BillPayment
{
    [Key] public int Id { get; set; }

    public int BillId { get; set; }
    [ForeignKey(nameof(BillId))] public Bill? Bill { get; set; }

    [MaxLength(20)]  public string Method { get; set; } = "Cash"; // Cash / UPI / Card / Credit
    [MaxLength(100)] public string? Reference { get; set; }

    [Column(TypeName = "decimal(18,2)")] public decimal Amount { get; set; }

    public DateTime At { get; set; } = DateTime.UtcNow;
}
