using System.IO;
using System.Text;
using System.Xml;
using Microsoft.EntityFrameworkCore;
using StoreAssistantProfessional.Data;

namespace StoreAssistantProfessional.Services;

public interface ITallyExportService
{
    Task<string> ExportSalesAsync(DateTime from, DateTime to);
}

public sealed class TallyExportService : ITallyExportService
{
    private readonly IDbContextFactory<AppDbContext> _dbFactory;

    public TallyExportService(IDbContextFactory<AppDbContext> dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public async Task<string> ExportSalesAsync(DateTime from, DateTime to)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var firm = await db.Firms.AsNoTracking().FirstOrDefaultAsync();
        var bills = await db.Bills
            .Include(b => b.Items).ThenInclude(i => i.TaxRate)
            .Include(b => b.Payments)
            .Include(b => b.Customer)
            .AsNoTracking()
            .Where(b => b.Status == "Posted" && b.At >= from && b.At < to)
            .OrderBy(b => b.At)
            .ToListAsync();

        var dir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "StoreAssistantProfessional", "TallyExports");
        Directory.CreateDirectory(dir);
        var path = Path.Combine(dir, $"sales-{from:yyyyMMdd}-{to.AddDays(-1):yyyyMMdd}.xml");

        var settings = new XmlWriterSettings { Indent = true, Encoding = Encoding.UTF8 };
        await using var sw = new StreamWriter(path, false, Encoding.UTF8);
        using var w = XmlWriter.Create(sw, settings);

        w.WriteStartDocument();
        w.WriteStartElement("ENVELOPE");

        w.WriteStartElement("HEADER");
        w.WriteElementString("VERSION", "1");
        w.WriteElementString("TALLYREQUEST", "Import");
        w.WriteElementString("TYPE", "Data");
        w.WriteElementString("ID", "Vouchers");
        w.WriteEndElement();

        w.WriteStartElement("BODY");
        w.WriteStartElement("DESC");
        w.WriteEndElement();
        w.WriteStartElement("DATA");
        w.WriteStartElement("TALLYMESSAGE");

        foreach (var b in bills)
        {
            w.WriteStartElement("VOUCHER");
            w.WriteAttributeString("REMOTEID", b.Number);
            w.WriteAttributeString("VCHTYPE", "Sales");
            w.WriteAttributeString("ACTION", "Create");

            w.WriteElementString("DATE", b.At.ToString("yyyyMMdd"));
            w.WriteElementString("VOUCHERTYPENAME", "Sales");
            w.WriteElementString("VOUCHERNUMBER", b.Number);
            w.WriteElementString("PARTYLEDGERNAME", b.Customer?.Name ?? "Walk-in");
            w.WriteElementString("REFERENCE", b.Number);
            w.WriteElementString("NARRATION", b.Notes ?? "");

            // Party (debit) entry — total
            w.WriteStartElement("ALLLEDGERENTRIES.LIST");
            w.WriteElementString("LEDGERNAME", b.Customer?.Name ?? "Walk-in");
            w.WriteElementString("ISDEEMEDPOSITIVE", "Yes");
            w.WriteElementString("AMOUNT", $"-{b.Total:F2}");
            w.WriteEndElement();

            // Sales (credit) entry — net
            var net = b.Subtotal - b.DiscountTotal;
            w.WriteStartElement("ALLLEDGERENTRIES.LIST");
            w.WriteElementString("LEDGERNAME", "Sales");
            w.WriteElementString("ISDEEMEDPOSITIVE", "No");
            w.WriteElementString("AMOUNT", $"{net:F2}");
            w.WriteEndElement();

            // GST split
            if (b.TaxTotal > 0)
            {
                w.WriteStartElement("ALLLEDGERENTRIES.LIST");
                w.WriteElementString("LEDGERNAME", "Output CGST");
                w.WriteElementString("ISDEEMEDPOSITIVE", "No");
                w.WriteElementString("AMOUNT", $"{(b.TaxTotal / 2):F2}");
                w.WriteEndElement();

                w.WriteStartElement("ALLLEDGERENTRIES.LIST");
                w.WriteElementString("LEDGERNAME", "Output SGST");
                w.WriteElementString("ISDEEMEDPOSITIVE", "No");
                w.WriteElementString("AMOUNT", $"{(b.TaxTotal / 2):F2}");
                w.WriteEndElement();
            }

            // Inventory entries
            foreach (var item in b.Items)
            {
                w.WriteStartElement("INVENTORYENTRIES.LIST");
                w.WriteElementString("STOCKITEMNAME", item.Description);
                w.WriteElementString("ACTUALQTY", $"{item.Quantity:F3}");
                w.WriteElementString("BILLEDQTY", $"{item.Quantity:F3}");
                w.WriteElementString("RATE", $"{item.UnitPrice:F2}");
                w.WriteElementString("AMOUNT", $"{item.LineTotal:F2}");
                if (!string.IsNullOrEmpty(item.HsnCode))
                    w.WriteElementString("HSNCODE", item.HsnCode);
                w.WriteEndElement();
            }

            w.WriteEndElement(); // VOUCHER
        }

        w.WriteEndElement(); // TALLYMESSAGE
        w.WriteEndElement(); // DATA
        w.WriteEndElement(); // BODY
        w.WriteEndElement(); // ENVELOPE
        w.WriteEndDocument();

        return path;
    }
}
