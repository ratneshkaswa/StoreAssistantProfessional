using System.Text.RegularExpressions;

namespace StoreAssistantProfessional.Validation;

// Format validators for Indian retail identifiers. Each predicate returns true
// for null/empty (treat as "not provided"); use a separate required-check at the
// call site if a field is mandatory.
public static partial class IndianFormats
{
    [GeneratedRegex(@"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z][1-9A-Z]Z[0-9A-Z]$")]
    private static partial Regex GstinRegex();

    [GeneratedRegex(@"^[A-Z]{5}[0-9]{4}[A-Z]$")]
    private static partial Regex PanRegex();

    [GeneratedRegex(@"^[A-Z]{4}0[A-Z0-9]{6}$")]
    private static partial Regex IfscRegex();

    [GeneratedRegex(@"^[1-9][0-9]{5}$")]
    private static partial Regex PincodeRegex();

    [GeneratedRegex(@"^[6-9][0-9]{9}$")]
    private static partial Regex IndianMobileRegex();

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    private static partial Regex EmailRegex();

    [GeneratedRegex(@"^[A-Z0-9]+$")]
    private static partial Regex InvoicePrefixRegex();

    [GeneratedRegex(@"^[0-9]{2,8}$")]
    private static partial Regex HsnRegex();

    public static bool IsBlank(string? s) => string.IsNullOrWhiteSpace(s);

    public static bool IsGstin(string? s) =>
        IsBlank(s) || GstinRegex().IsMatch(s!.Trim().ToUpperInvariant());

    public static bool IsPan(string? s) =>
        IsBlank(s) || PanRegex().IsMatch(s!.Trim().ToUpperInvariant());

    public static bool IsIfsc(string? s) =>
        IsBlank(s) || IfscRegex().IsMatch(s!.Trim().ToUpperInvariant());

    public static bool IsPincode(string? s) =>
        IsBlank(s) || PincodeRegex().IsMatch(s!.Trim());

    public static bool IsIndianPhone(string? s)
    {
        if (IsBlank(s)) return true;
        var digits = StripPhone(s!);
        return IndianMobileRegex().IsMatch(digits);
    }

    public static bool IsEmail(string? s) =>
        IsBlank(s) || EmailRegex().IsMatch(s!.Trim());

    public static bool IsInvoicePrefix(string? s) =>
        IsBlank(s) || InvoicePrefixRegex().IsMatch(s!.Trim().ToUpperInvariant());

    public static bool IsHsn(string? s) =>
        IsBlank(s) || HsnRegex().IsMatch(s!.Trim());

    // Strips spaces, dashes, parens, and a leading +91 / 91 / 0 country/trunk prefix
    // so user input like "+91 98xxx-xxxxx" or "098xxxxxxxxx" matches a stored 10-digit number.
    public static string StripPhone(string s)
    {
        var t = s.Trim();
        var buf = new System.Text.StringBuilder(t.Length);
        foreach (var c in t)
            if (c is >= '0' and <= '9') buf.Append(c);
        var d = buf.ToString();
        if (d.StartsWith("91") && d.Length == 12) d = d[2..];
        else if (d.StartsWith("0") && d.Length == 11) d = d[1..];
        return d;
    }

    // GSTIN encodes the registration state in its first two digits.
    public static string? StateCodeFromGstin(string? gstin)
    {
        if (IsBlank(gstin)) return null;
        var t = gstin!.Trim().ToUpperInvariant();
        if (t.Length < 2) return null;
        var code = t[..2];
        return code.All(c => c is >= '0' and <= '9') ? code : null;
    }

    // Maps the 2-digit GSTIN state code to its state name. Covers all current
    // Indian states + UTs. Returns null for unknown codes (incl. 38+ reserved).
    public static string? StateNameFromCode(string? code) => code switch
    {
        "01" => "Jammu and Kashmir",
        "02" => "Himachal Pradesh",
        "03" => "Punjab",
        "04" => "Chandigarh",
        "05" => "Uttarakhand",
        "06" => "Haryana",
        "07" => "Delhi",
        "08" => "Rajasthan",
        "09" => "Uttar Pradesh",
        "10" => "Bihar",
        "11" => "Sikkim",
        "12" => "Arunachal Pradesh",
        "13" => "Nagaland",
        "14" => "Manipur",
        "15" => "Mizoram",
        "16" => "Tripura",
        "17" => "Meghalaya",
        "18" => "Assam",
        "19" => "West Bengal",
        "20" => "Jharkhand",
        "21" => "Odisha",
        "22" => "Chhattisgarh",
        "23" => "Madhya Pradesh",
        "24" => "Gujarat",
        "25" => "Daman and Diu",
        "26" => "Dadra and Nagar Haveli",
        "27" => "Maharashtra",
        "28" => "Andhra Pradesh (Old)",
        "29" => "Karnataka",
        "30" => "Goa",
        "31" => "Lakshadweep",
        "32" => "Kerala",
        "33" => "Tamil Nadu",
        "34" => "Puducherry",
        "35" => "Andaman and Nicobar",
        "36" => "Telangana",
        "37" => "Andhra Pradesh",
        "38" => "Ladakh",
        _ => null,
    };
}
