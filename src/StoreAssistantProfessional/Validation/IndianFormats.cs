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

    // Strips spaces, dashes, parens, dots, and a leading +91 / 91 / 0 country/trunk
    // prefix so user input like "+91 98xxx-xxxxx", "(98) 765.4321", or "098xxxxxxxxx"
    // collapses to the canonical 10-digit form. Everything non-digit goes; we don't
    // try to detect international numbers, only Indian.
    public static string StripPhone(string s)
    {
        if (string.IsNullOrEmpty(s)) return "";
        var buf = new System.Text.StringBuilder(s.Length);
        foreach (var c in s)
            if (c is >= '0' and <= '9') buf.Append(c);
        var d = buf.ToString();
        if (d.StartsWith("91") && d.Length == 12) d = d[2..];
        else if (d.StartsWith("0") && d.Length == 11) d = d[1..];
        return d;
    }

    // GSTIN checksum: positions 1-14 are the body; position 15 is a mod-36 check digit
    // computed by alternating weights of 1 and 2 over the base-36 value of each char,
    // summing the digit-sum of each weighted product, then taking 36 minus that sum mod 36.
    // Returns true for blank input (treated as "not provided"); only validates when shape
    // already passes IsGstin so we don't double-flag obviously-invalid input.
    public static bool IsGstinChecksumValid(string? gstin)
    {
        if (IsBlank(gstin)) return true;
        var g = gstin!.Trim().ToUpperInvariant();
        if (!GstinRegex().IsMatch(g)) return true; // shape error wins; defer to IsGstin
        const string alphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var sum = 0;
        for (var i = 0; i < 14; i++)
        {
            var v = alphabet.IndexOf(g[i]);
            if (v < 0) return false;
            var weight = (i % 2 == 0) ? 1 : 2;
            var prod = v * weight;
            sum += (prod / 36) + (prod % 36);
        }
        var check = (36 - (sum % 36)) % 36;
        return alphabet[check] == g[14];
    }

    // GSTIN encodes PAN at positions 3-12 (1-indexed) — i.e. characters 2..11 (0-indexed).
    // Returns null for blank or shape-invalid input so callers can branch on "did we
    // get something usable" without re-validating shape.
    public static string? PanFromGstin(string? gstin)
    {
        if (IsBlank(gstin)) return null;
        var g = gstin!.Trim().ToUpperInvariant();
        if (!GstinRegex().IsMatch(g)) return null;
        return g.Substring(2, 10);
    }

    // GSTIN encodes the registration state in its first two digits. Returns null
    // unless the code is in the known-state range (01-38) — typos like "99" used
    // to silently land here, then StateNameFromCode would return null and the form
    // would visibly clear. Now we reject up front.
    public static string? StateCodeFromGstin(string? gstin)
    {
        if (IsBlank(gstin)) return null;
        var t = gstin!.Trim().ToUpperInvariant();
        if (t.Length < 2) return null;
        var code = t[..2];
        if (!code.All(c => c is >= '0' and <= '9')) return null;
        return StateNameFromCode(code) is null ? null : code;
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
