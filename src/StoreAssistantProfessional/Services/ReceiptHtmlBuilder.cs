using System.Net;
using System.Text;
using StoreAssistantProfessional.Models;

namespace StoreAssistantProfessional.Services;

public static class ReceiptHtmlBuilder
{
    public static string Build(Bill bill, Firm? firm, AppSettings? settings = null)
    {
        var sb = new StringBuilder();
        var lang = settings?.ReceiptLanguage ?? "en";
        var bilingual = settings?.ReceiptBilingual ?? false;
        var docTitle = lang == "hi" ? KindHindi(bill) : KindEnglish(bill);
        if (bilingual && lang == "hi")
            docTitle = $"{KindEnglish(bill)} · {docTitle}";

        sb.Append("<!doctype html><html><head><meta charset='utf-8'><title>").Append(WebUtility.HtmlEncode(docTitle)).Append(' ').Append(bill.Number).Append("</title><style>");
        sb.Append("body{font-family:Consolas,monospace;width:80mm;padding:8px;font-size:12px;}");
        sb.Append("h2,h3{text-align:center;margin:4px 0;}");
        sb.Append("table{width:100%;border-collapse:collapse;}");
        sb.Append("td.r{text-align:right;}");
        sb.Append("hr{border:none;border-top:1px dashed #000;margin:6px 0;}");
        sb.Append(".tot{font-size:14px;font-weight:700;}");
        sb.Append(".doc{text-align:center;letter-spacing:2px;font-weight:700;font-size:11px;}");
        sb.Append("@media print{button{display:none;}}");
        sb.Append("</style></head><body>");

        sb.Append("<h3>").Append(WebUtility.HtmlEncode(firm?.Name ?? "Store")).Append("</h3>");
        if (!string.IsNullOrEmpty(firm?.AddressLine1)) sb.Append("<div style='text-align:center'>").Append(WebUtility.HtmlEncode(firm.AddressLine1)).Append("</div>");
        if (!string.IsNullOrEmpty(firm?.Gstin)) sb.Append("<div style='text-align:center'>GSTIN ").Append(firm.Gstin).Append("</div>");

        sb.Append("<div class='doc'>").Append(WebUtility.HtmlEncode(docTitle)).Append("</div>");

        if (bill.IsComposition)
            sb.Append("<div style='text-align:center;font-size:10px;'>Composition taxable person — not eligible to collect tax</div>");

        sb.Append("<hr>");
        sb.Append("<div>").Append(WebUtility.HtmlEncode(docTitle.Length <= 12 ? docTitle : "Doc")).Append(' ').Append(bill.Number)
          .Append(" · ").Append(bill.At.ToLocalTime().ToString("dd/MM/yyyy HH:mm")).Append("</div>");
        if (bill.Customer is not null) sb.Append("<div>Customer: ").Append(WebUtility.HtmlEncode(bill.Customer.Name)).Append("</div>");

        sb.Append("<hr><table>");
        foreach (var item in bill.Items)
        {
            sb.Append("<tr><td colspan='2'>").Append(WebUtility.HtmlEncode(item.Description)).Append("</td></tr>");
            sb.Append("<tr><td>").Append(item.Quantity.ToString("0.###")).Append(" × ₹").Append(item.UnitPrice.ToString("N2")).Append("</td>");
            sb.Append("<td class='r'>₹").Append(item.LineTotal.ToString("N2")).Append("</td></tr>");
        }
        sb.Append("</table><hr><table>");
        sb.Append("<tr><td>Subtotal</td><td class='r'>₹").Append(bill.Subtotal.ToString("N2")).Append("</td></tr>");
        if (bill.DiscountTotal > 0) sb.Append("<tr><td>Discount</td><td class='r'>−₹").Append(bill.DiscountTotal.ToString("N2")).Append("</td></tr>");
        if (!bill.IsComposition && bill.TaxTotal > 0)
        {
            sb.Append("<tr><td>CGST</td><td class='r'>₹").Append((bill.TaxTotal / 2).ToString("N2")).Append("</td></tr>");
            sb.Append("<tr><td>SGST</td><td class='r'>₹").Append((bill.TaxTotal / 2).ToString("N2")).Append("</td></tr>");
        }
        sb.Append("<tr class='tot'><td>TOTAL</td><td class='r'>₹").Append(bill.Total.ToString("N2")).Append("</td></tr></table>");

        if (bill.Payments.Count > 0)
        {
            sb.Append("<hr>");
            foreach (var p in bill.Payments)
                sb.Append("<div>").Append(p.Method).Append(": ₹").Append(p.Amount.ToString("N2")).Append("</div>");
            if (bill.ChangeAmount > 0) sb.Append("<div>Change: ₹").Append(bill.ChangeAmount.ToString("N2")).Append("</div>");
        }

        var thank = lang == "hi" ? "धन्यवाद" : "Thank you";
        sb.Append("<hr><div style='text-align:center'>").Append(thank).Append("</div></body></html>");
        return sb.ToString();
    }

    private static string KindEnglish(Bill bill) => bill.Kind switch
    {
        "Quote" => "QUOTATION",
        "Proforma" => "PROFORMA INVOICE",
        "DeliveryChallan" => "DELIVERY CHALLAN",
        "CashReceipt" => "CASH RECEIPT",
        _ => bill.IsComposition ? "BILL OF SUPPLY" : "TAX INVOICE"
    };

    private static string KindHindi(Bill bill) => bill.Kind switch
    {
        "Quote" => "मूल्य उद्धरण",
        "Proforma" => "प्रोफॉर्मा बिल",
        "DeliveryChallan" => "डिलीवरी चालान",
        "CashReceipt" => "नकद रसीद",
        _ => bill.IsComposition ? "आपूर्ति बिल" : "कर बीजक"
    };
}
