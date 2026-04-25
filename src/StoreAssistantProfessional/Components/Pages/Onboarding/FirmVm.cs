using StoreAssistantProfessional.Models;
using StoreAssistantProfessional.Validation;

namespace StoreAssistantProfessional.Components.Pages.Onboarding;

// Form-side projection of `Firm`. Reasons to exist instead of binding to Firm
// directly: (1) we need a single Normalize() pass that trims, upper/lower-cases,
// and strips phone formatting before save, (2) we need to round-trip Logo*
// fields that no UI control on this page edits — without them in the VM,
// ToEntity() would write nulls and wipe a logo uploaded elsewhere, (3) the
// page mutates fields with cross-effects (HasGstRegistration false ⇒ Gstin
// must clear) that don't belong on the persisted entity.
internal sealed class FirmVm
{
    public int Id { get; set; } = Firm.SingletonId;
    public string Name { get; set; } = "";

    // Explicit toggle controlling whether Gstin is editable on the form.
    // Normalize() blanks Gstin when this is false so a previously-entered
    // value doesn't linger as ghost data.
    public bool HasGstRegistration { get; set; }

    public string? Gstin { get; set; }
    public string? Pan { get; set; }
    public string? StateCode { get; set; }
    public string? StateName { get; set; }

    // Round-tripped through the form so saving doesn't clobber a logo that was
    // uploaded via a different page. Not edited here.
    public string? LogoBase64 { get; set; }
    public string? LogoMimeType { get; set; }

    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? AddressLine1 { get; set; }
    public string? City { get; set; }
    public string? Pincode { get; set; }
    public string? BankName { get; set; }
    public string? AccountHolderName { get; set; }
    public string? AccountNumber { get; set; }
    public string? IfscCode { get; set; }
    public string? BranchName { get; set; }
    public string? UpiId { get; set; }
    public int FyStartDay { get; set; } = 1;
    public int FyStartMonth { get; set; } = 4;

    // Composition-scheme is set on the Tax onboarding step. The VM round-trips
    // the field so saves from FirmDetailsPage don't blank a value the user set
    // on TaxRatesPage.
    public bool IsCompositionScheme { get; set; }

    public static FirmVm From(Firm f)
    {
        var month = f.FyStartMonth is < 1 or > 12 ? 4 : f.FyStartMonth;
        // Reference year 2024 is a leap year — gives Feb its full 29 days. Using
        // DateTime.Today.Year here would silently drop day 29 in non-leap years.
        var maxDay = DateTime.DaysInMonth(2024, month);
        var day = Math.Clamp(f.FyStartDay <= 0 ? 1 : f.FyStartDay, 1, maxDay);

        return new FirmVm
        {
            Id = f.Id,
            Name = f.Name ?? "",
            HasGstRegistration = f.HasGstRegistration,
            Gstin = f.Gstin,
            Pan = f.Pan,
            StateCode = f.StateCode,
            StateName = f.StateName,
            LogoBase64 = f.LogoBase64,
            LogoMimeType = f.LogoMimeType,
            Phone = f.Phone,
            Email = f.Email,
            AddressLine1 = f.AddressLine1,
            City = f.City,
            Pincode = f.Pincode,
            BankName = f.BankName,
            AccountHolderName = f.AccountHolderName,
            AccountNumber = f.AccountNumber,
            IfscCode = f.IfscCode,
            BranchName = f.BranchName,
            UpiId = f.UpiId,
            FyStartDay = day,
            FyStartMonth = month,
            IsCompositionScheme = f.IsCompositionScheme,
        };
    }

    public Firm ToEntity() => new()
    {
        Id = Id,
        Name = Name,
        HasGstRegistration = HasGstRegistration,
        Gstin = Gstin,
        Pan = Pan,
        StateCode = StateCode,
        StateName = StateName,
        LogoBase64 = LogoBase64,
        LogoMimeType = LogoMimeType,
        Phone = Phone,
        Email = Email,
        AddressLine1 = AddressLine1,
        City = City,
        Pincode = Pincode,
        BankName = BankName,
        AccountHolderName = AccountHolderName,
        AccountNumber = AccountNumber,
        IfscCode = IfscCode,
        BranchName = BranchName,
        UpiId = UpiId,
        FyStartDay = FyStartDay,
        FyStartMonth = FyStartMonth,
        IsCompositionScheme = IsCompositionScheme,
    };

    // Single normalization pass before save: trim every string, upper-case the
    // identifiers, lower-case email/UPI, strip phone formatting to canonical
    // 10-digit, snap FY day/month back into their valid sets, and blank Gstin
    // / state-derived-from-Gstin when the GST toggle is off so we don't
    // persist orphan values the user can't see.
    public void Normalize()
    {
        Name = (Name ?? "").Trim();

        if (!HasGstRegistration)
        {
            Gstin = null;
        }
        else
        {
            Gstin = NormalizeUpper(Gstin);
        }

        Pan = NormalizeUpper(Pan);
        StateCode = NormalizeUpper(StateCode);
        StateName = TrimToNull(StateName);
        Phone = NormalizePhone(Phone);
        Email = NormalizeLower(Email);
        AddressLine1 = TrimToNull(AddressLine1);
        City = TrimToNull(City);
        Pincode = TrimToNull(Pincode);
        BankName = TrimToNull(BankName);
        AccountHolderName = TrimToNull(AccountHolderName);
        AccountNumber = TrimToNull(AccountNumber);
        IfscCode = NormalizeUpper(IfscCode);
        BranchName = TrimToNull(BranchName);
        UpiId = NormalizeLower(UpiId);

        FyStartMonth = FyStartMonth is < 1 or > 12 ? 4 : FyStartMonth;
        FyStartDay = Math.Clamp(FyStartDay, 1, DateTime.DaysInMonth(2024, FyStartMonth));
    }

    public bool IsValid =>
        !string.IsNullOrWhiteSpace(Name) &&
        Name.Length <= 200 &&
        // GSTIN only validated when the toggle is on. With it off, any
        // previously-typed value is ignored (Normalize will blank it).
        (!HasGstRegistration || (IndianFormats.IsGstin(Gstin) && !string.IsNullOrWhiteSpace(Gstin))) &&
        IndianFormats.IsPan(Pan) &&
        IsStateCodeShapeValid &&
        IsPanGstinConsistent &&
        IndianFormats.IsIndianPhone(Phone) &&
        IndianFormats.IsEmail(Email) &&
        IndianFormats.IsPincode(Pincode) &&
        IndianFormats.IsIfsc(IfscCode) &&
        IsBankInfoConsistent;

    // Receipts pull bank info as a unit. If any of the four primary fields is
    // set, all four must be set — receipts with half-typed bank details look
    // broken. Empty-all is fine (bank info is optional overall).
    public bool IsBankInfoConsistent
    {
        get
        {
            var any = !string.IsNullOrWhiteSpace(BankName)
                   || !string.IsNullOrWhiteSpace(AccountHolderName)
                   || !string.IsNullOrWhiteSpace(AccountNumber)
                   || !string.IsNullOrWhiteSpace(IfscCode);
            if (!any) return true;
            return !string.IsNullOrWhiteSpace(AccountNumber)
                && !string.IsNullOrWhiteSpace(IfscCode);
        }
    }

    public bool IsStateCodeShapeValid =>
        string.IsNullOrWhiteSpace(StateCode) ||
        (StateCode.Length == 2 && StateCode.All(char.IsDigit));

    // PAN is embedded in GSTIN[2..12]. Mismatch indicates a typo. Skipped when
    // the GST toggle is off (GSTIN should be ignored), or when either field is
    // shape-invalid (the field-level errors take priority).
    public bool IsPanGstinConsistent
    {
        get
        {
            if (!HasGstRegistration) return true;
            if (string.IsNullOrWhiteSpace(Gstin) || string.IsNullOrWhiteSpace(Pan)) return true;
            var g = Gstin.Trim().ToUpperInvariant();
            var p = Pan.Trim().ToUpperInvariant();
            if (g.Length != 15 || p.Length != 10) return true;
            return g.Substring(2, 10) == p;
        }
    }

    // Computes the FY window containing `today` using the configured start
    // day/month. Pair-clamped both for `today.Year` (so a Feb-29 start in a
    // non-leap year still resolves) and for the next-year boundary. Format
    // is hardcoded `dd MMM yyyy` (no per-firm DateFormat configuration any
    // more — the date format used in invoice numbers is also fixed).
    public (DateTime Start, DateTime End, string Label) GetFyForToday(DateTime today)
    {
        var month = FyStartMonth is < 1 or > 12 ? 4 : FyStartMonth;
        var day = Math.Clamp(FyStartDay, 1, DateTime.DaysInMonth(today.Year, month));
        var startThisYear = new DateTime(today.Year, month, day);
        var fyStart = today >= startThisYear ? startThisYear : startThisYear.AddYears(-1);
        var fyEnd = fyStart.AddYears(1).AddDays(-1);
        var span = fyStart.Year == fyEnd.Year ? $"FY {fyStart.Year}" : $"FY {fyStart.Year}-{(fyEnd.Year % 100):D2}";
        return (fyStart, fyEnd, $"{span} ({fyStart:dd MMM yyyy} → {fyEnd:dd MMM yyyy})");
    }

    private static string? TrimToNull(string? s) =>
        string.IsNullOrWhiteSpace(s) ? null : s.Trim();

    private static string? NormalizeUpper(string? s) =>
        string.IsNullOrWhiteSpace(s) ? null : s.Trim().ToUpperInvariant();

    private static string? NormalizeLower(string? s) =>
        string.IsNullOrWhiteSpace(s) ? null : s.Trim().ToLowerInvariant();

    private static string? NormalizePhone(string? s)
    {
        if (string.IsNullOrWhiteSpace(s)) return null;
        var stripped = IndianFormats.StripPhone(s);
        return string.IsNullOrEmpty(stripped) ? s.Trim() : stripped;
    }
}
