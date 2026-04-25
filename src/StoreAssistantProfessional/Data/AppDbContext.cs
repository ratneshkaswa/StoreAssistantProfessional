using System.IO;
using Microsoft.EntityFrameworkCore;
using StoreAssistantProfessional.Models;

namespace StoreAssistantProfessional.Data;

public sealed class AppDbContext : DbContext
{
    public DbSet<Firm> Firms => Set<Firm>();
    public DbSet<AppSettings> Settings => Set<AppSettings>();
    public DbSet<TaxRate> TaxRates => Set<TaxRate>();
    public DbSet<Vendor> Vendors => Set<Vendor>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Staff> Staff => Set<Staff>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<InwardEntry> InwardEntries => Set<InwardEntry>();
    public DbSet<InwardParcel> InwardParcels => Set<InwardParcel>();
    public DbSet<InwardParcelItem> InwardParcelItems => Set<InwardParcelItem>();
    public DbSet<SessionEvent> SessionEvents => Set<SessionEvent>();
    public DbSet<StockAdjustment> StockAdjustments => Set<StockAdjustment>();
    public DbSet<PurchaseReturn> PurchaseReturns => Set<PurchaseReturn>();
    public DbSet<PurchaseReturnItem> PurchaseReturnItems => Set<PurchaseReturnItem>();
    public DbSet<Bill> Bills => Set<Bill>();
    public DbSet<BillItem> BillItems => Set<BillItem>();
    public DbSet<BillPayment> BillPayments => Set<BillPayment>();
    public DbSet<Expense> Expenses => Set<Expense>();
    public DbSet<RecurringExpense> RecurringExpenses => Set<RecurringExpense>();
    public DbSet<Cheque> Cheques => Set<Cheque>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public static string DefaultDbPath => Path.Combine(AppPaths.AppDataDir, "store.db");

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Firm>().Property(p => p.Id).ValueGeneratedNever();
        b.Entity<AppSettings>().Property(p => p.Id).ValueGeneratedNever();

        // Filtered unique indexes ignore soft-deleted (IsActive=0) rows so a user
        // who removed an entry can recreate it. NOCASE collation on Name columns
        // makes "GST 18%" and "gst 18%" collide as duplicates, and lets
        // OrderBy(x => x.Name) sort case-insensitively in SQLite (whose default
        // BINARY collation otherwise puts "abc" after "ZBC").
        b.Entity<TaxRate>().Property(t => t.Name).UseCollation("NOCASE");
        b.Entity<TaxRate>().HasIndex(t => t.Name).IsUnique().HasFilter("\"IsActive\" = 1");
        b.Entity<TaxRate>().HasIndex(t => t.IsDefault);

        b.Entity<Vendor>().Property(v => v.Name).UseCollation("NOCASE");
        b.Entity<Vendor>().HasIndex(v => v.Name);
        b.Entity<Vendor>().HasIndex(v => v.Phone);

        b.Entity<Customer>().Property(c => c.Name).UseCollation("NOCASE");
        b.Entity<Customer>().HasIndex(c => c.Phone);
        b.Entity<Customer>().HasIndex(c => c.Name);

        b.Entity<Staff>().HasIndex(s => s.Code).IsUnique();
        b.Entity<Staff>().HasIndex(s => s.Name);

        b.Entity<Product>().Property(p => p.Name).UseCollation("NOCASE");
        // SKU index is filtered: soft-deleted SKUs must not block a fresh entry
        // (same shape as the TaxRate fix).
        b.Entity<Product>().HasIndex(p => p.Sku).IsUnique().HasFilter("\"IsActive\" = 1");
        b.Entity<Product>().HasIndex(p => p.Barcode);
        b.Entity<Product>().HasIndex(p => p.Name);

        b.Entity<InwardEntry>().HasIndex(i => i.InwardNumber).IsUnique();
        b.Entity<InwardEntry>().HasIndex(i => i.EntryDate);

        b.Entity<SessionEvent>().HasIndex(e => e.At);

        b.Entity<StockAdjustment>().HasIndex(a => a.At);
        b.Entity<StockAdjustment>().HasIndex(a => a.BatchTag);

        b.Entity<PurchaseReturn>().HasIndex(p => p.ReturnNumber).IsUnique();
        b.Entity<PurchaseReturn>().HasIndex(p => p.At);

        b.Entity<PurchaseReturnItem>()
            .HasOne(i => i.PurchaseReturn)
            .WithMany(p => p.Items)
            .HasForeignKey(i => i.PurchaseReturnId)
            .OnDelete(DeleteBehavior.Cascade);

        b.Entity<Bill>().HasIndex(x => x.Number).IsUnique();
        b.Entity<Bill>().HasIndex(x => x.At);
        b.Entity<Bill>().HasIndex(x => x.Status);

        b.Entity<BillItem>()
            .HasOne(i => i.Bill).WithMany(x => x.Items)
            .HasForeignKey(i => i.BillId).OnDelete(DeleteBehavior.Cascade);

        b.Entity<BillPayment>()
            .HasOne(p => p.Bill).WithMany(x => x.Payments)
            .HasForeignKey(p => p.BillId).OnDelete(DeleteBehavior.Cascade);

        b.Entity<Expense>().HasIndex(e => e.At);
        b.Entity<Expense>().HasIndex(e => e.Category);

        b.Entity<RecurringExpense>().HasIndex(r => r.NextDue);
        b.Entity<RecurringExpense>().HasIndex(r => r.IsActive);

        b.Entity<Cheque>().HasIndex(c => c.Status);
        b.Entity<Cheque>().HasIndex(c => c.IssuedAt);
        b.Entity<Cheque>().HasIndex(c => c.Number);

        b.Entity<InwardParcel>()
            .HasOne(p => p.InwardEntry)
            .WithMany(e => e.Parcels)
            .HasForeignKey(p => p.InwardEntryId)
            .OnDelete(DeleteBehavior.Cascade);

        b.Entity<InwardParcelItem>()
            .HasOne(i => i.InwardParcel)
            .WithMany(p => p.Items)
            .HasForeignKey(i => i.InwardParcelId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
