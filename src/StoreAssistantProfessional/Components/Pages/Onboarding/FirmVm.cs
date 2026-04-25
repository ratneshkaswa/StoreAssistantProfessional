using StoreAssistantProfessional.Models;
using StoreAssistantProfessional.Validation;

namespace StoreAssistantProfessional.Components.Pages.Onboarding;

// Form-side projection of `Firm`. Reasons to exist instead of binding to Firm
// directly: (1) MudNumericField wants a nullable backing for clear-to-empty,
// (2) we need a single Normalize() pass that trims, upper/lower-cases, and
// strips phone formatting before save, (3) we need to round-trip Logo* fields
// that no UI control on this page edits — without them in the VM, ToEntity()
// would write nulls and wipe a logo uploaded elsewhere.
internal sealed class FirmVm
{
    // Whitelisted UI choices. Saving anything outside these sets means somebody
    // edited the database directly; From() falls back rather than carrying the
    // bad value into the form (where dropdowns would render blank).
    public static readonly string[] AllowedDateFormats =
        ["dd/MM/yyyy", "dd-MM-yyyy", "yyyy-MM-dd", "MM/dd/yyyy", "dd MMM yyyy"];

    public static readonly string[] AllowedPaddings =
        ["0", "00", "000", "0000", "00000", "000000"];

    public int Id { get; set; } = Firm.SingletonId;
    public string Name { get; set; } = "";
    public string? Gstin { get; set; }
    public string? Pan { get; set; }
    public string? StateCode { get; set; }
    public string? StateName { get; set; }

    // Round-tripped through the form so saving doesn't clobber a logo that was
    // uploaded via a different page. Not edited here yet.
    public string? LogoBase64 { get; set; }
    public string? LogoMimeType { get; set; }

    public string? Phone { get; set; }
    public string? AltPhone { get; set; }
    public string? Email { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? City { get; set; }
    public string? Pincode { get; set; }
    public string? BankName { get; set; }
    public string? AccountHolderName { get; set; }
    public string? AccountNumber { get; set; }
    public string? IfscCode { get; set; }
    public string? BranchName { get; set; }
    public string? UpiId { get; set; }
    public string InvoicePrefix { get; set; } = "INV";
    public int InvoiceStartNumber { get; set; } = 1;
    public int? InvoiceStartNumberNullable
    {
        get => InvoiceStartNumber;
        set => InvoiceStartNumber = value is null or < 1 ? 1 : value.Value;
    }
    public int NextInvoiceNumber { get; set; } = 1;
    public string InvoiceNumberPadding { get; set; } = "0000";
    public bool ResetInvoiceOnFyRollover { get; set; } = true;
    public string DateFormat { get; set; } = "dd/MM/yyyy";
    public int FyStartDay { get; set; } = 1;
    public int FyStartMonth { get; set; } = 4;
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
            Gstin = f.Gstin,
            Pan = f.Pan,
            StateCode = f.StateCode,
            StateName = f.StateName,
            LogoBase64 = f.LogoBase64,
            LogoMimeType = f.LogoMimeType,
            Phone = f.Phone,
            AltPhone = f.AltPhone,
            Email = f.Email,
            AddressLine1 = f.AddressLine1,
            AddressLine2 = f.AddressLine2,
            City = f.City,
            Pincode = f.Pincode,
            BankName = f.BankName,
            AccountHolderName = f.AccountHolderName,
            AccountNumber = f.AccountNumber,
            IfscCode = f.IfscCode,
            BranchName = f.BranchName,
            UpiId = f.UpiId,
            InvoicePrefix = string.IsNullOrWhiteSpace(f.InvoicePrefix) ? "INV" : f.InvoicePrefix,
            InvoiceStartNumber = f.InvoiceStartNumber <= 0 ? 1 : f.InvoiceStartNumber,
            NextInvoiceNumber = f.NextInvoiceNumber <= 0 ? 1 : f.NextInvoiceNumber,
            InvoiceNumberPadding = AllowedPaddings.Contains(f.InvoiceNumberPadding) ? f.InvoiceNumberPadding : "0000",
            ResetInvoiceOnFyRollover = f.ResetInvoiceOnFyRollover,
            DateFormat = AllowedDateFormats.Contains(f.DateFormat) ? f.DateFormat : "dd/MM/yyyy",
            FyStartDay = day,
            FyStartMonth = month,
            IsCompositionScheme = f.IsCompositionScheme,
        };
    }

    public Firm ToEntity() => new()
    {
        Id = Id,
        Name = Name,
        Gstin = Gstin,
        Pan = Pan,
        StateCode = StateCode,
        StateName = StateName,
        LogoBase64 = LogoBase64,
        LogoMimeType = LogoMimeType,
        Phone = Phone,
        AltPhone = AltPhone,
        Email = Email,
        AddressLine1 = AddressLine1,
        AddressLine2 = AddressLine2,
        City = City,
        Pincode = Pincode,
        BankName = BankName,
        AccountHolderName = AccountHolderName,
        AccountNumber = AccountNumber,
        IfscCode = IfscCode,
        BranchName = BranchName,
        UpiId = UpiId,
        InvoicePrefix = InvoicePrefix,
        InvoiceStartNumber = InvoiceStartNumber,
        NextInvoiceNumber = NextInvoiceNumber,
        InvoiceNumberPadding = InvoiceNumberPadding,
        ResetInvoiceOnFyRollover = ResetInvoiceOnFyRollover,
        DateFormat = DateFormat,
        FyStartDay = FyStartDay,
        FyStartMonth = FyStartMonth,
        IsCompositionScheme = IsCompositionScheme,
    };

    // Single normalization pass before save: trim every string, upper-case the
    // identifiers, lower-case email/UPI, strip phone formatting to canonical
    // 10-digit, pin a non-empty InvoicePrefix, snap padding/date format and
    // FY day/month back into their valid sets.
    public void Normalize()
    {
        Name = (Name ?? "").Trim();
        Gstin = NormalizeUpper(Gstin);
        Pan = NormalizeUpper(Pan);
        StateCode = NormalizeUpper(StateCode);
        StateName = TrimToNull(StateName);
        Phone = NormalizePhone(Phone);
        AltPhone = NormalizePhone(AltPhone);
        Email = NormalizeLower(Email);
        AddressLine1 = TrimToNull(AddressLine1);
        AddressLine2 = TrimToNull(AddressLine2);
        City = TrimToNull(City);
        Pincode = TrimToNull(Pincode);
        BankName = TrimToNull(BankName);
        AccountHolderName = TrimToNull(AccountHolderName);
        AccountNumber = TrimToNull(AccountNumber);
        IfscCode = NormalizeUpper(IfscCode);
        BranchName = TrimToNull(BranchName);
        UpiId = NormalizeLower(UpiId);

        var prefix = (InvoicePrefix ?? "").Trim();
        InvoicePrefix = string.IsNullOrEmpty(prefix) ? "INV" : prefix.ToUpperInvariant();

        if (!AllowedPaddings.Contains(InvoiceNumberPadding)) InvoiceNumberPadding = "0000";
        if (!AllowedDateFormats.Contains(DateFormat)) DateFormat = "dd/MM/yyyy";

        FyStartMonth = FyStartMonth is < 1 or > 12 ? 4 : FyStartMonth;
        FyStartDay = Math.Clamp(FyStartDay, 1, DateTime.DaysInMonth(2024, FyStartMonth));
    }

    public bool IsValid =>
        !string.IsNullOrWhiteSpace(Name) &&
        Name.Length <= 200 &&
        IndianFormats.IsGstin(Gstin) &&
        IndianFormats.IsPan(Pan) &&
        IsStateCodeShapeValid &&
        IsPanGstinConsistent &&
        // F2: composition firms must have a GSTIN — they issue Bills of Supply
        // with a GSTIN reference, so save is blocked when the box is ticked
        // and the field is blank. (Cosmetic warning was already shown.)
        !CompositionWithoutGstin &&
        IndianFormats.IsIndianPhone(Phone) &&
        IndianFormats.IsIndianPhone(AltPhone) &&
        IndianFormats.IsEmail(Email) &&
        IndianFormats.IsPincode(Pincode) &&
        IndianFormats.IsIfsc(IfscCode) &&
        IsBankInfoConsistent &&
        IsInvoicePrefixValid &&
        AllowedPaddings.Contains(InvoiceNumberPadding) &&
        AllowedDateFormats.Contains(DateFormat);

    // F8 / F23: receipts pull bank info as a unit. If any of the four primary
    // fields is set, all four must be set — receipts with half-typed bank
    // details look broken. Empty-all is fine (bank info is optional overall).
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
    // either field is shape-invalid — the field-level errors take priority.
    public bool IsPanGstinConsistent
    {
        get
        {
            if (string.IsNullOrWhiteSpace(Gstin) || string.IsNullOrWhiteSpace(Pan)) return true;
            var g = Gstin.Trim().ToUpperInvariant();
            var p = Pan.Trim().ToUpperInvariant();
            if (g.Length != 15 || p.Length != 10) return true;
            return g.Substring(2, 10) == p;
        }
    }

    public bool IsInvoicePrefixValid
    {
        get
        {
            var p = (InvoicePrefix ?? "").Trim().ToUpperInvariant();
            if (string.IsNullOrEmpty(p)) return false;
            if (!IndianFormats.IsInvoicePrefix(p)) return false;
            return !Firm.ReservedInvoicePrefixes.Contains(p);
        }
    }

    public bool CompositionWithoutGstin =>
        IsCompositionScheme && string.IsNullOrWhiteSpace(Gstin);

    // Computes the FY window containing `today` using the configured start
    // day/month. Pair-clamped both for `today.Year` (so a Feb-29 start in a
    // non-leap year still resolves) and for the next-year boundary.
    public (DateTime Start, DateTime End, string Label) GetFyForToday(DateTime today)
    {
        var month = FyStartMonth is < 1 or > 12 ? 4 : FyStartMonth;
        var day = Math.Clamp(FyStartDay, 1, DateTime.DaysInMonth(today.Year, month));
        var startThisYear = new DateTime(today.Year, month, day);
        var fyStart = today >= startThisYear ? startThisYear : startThisYear.AddYears(-1);
        var fyEnd = fyStart.AddYears(1).AddDays(-1);
        var fmt = AllowedDateFormats.Contains(DateFormat) ? DateFormat : "dd/MM/yyyy";
        var span = fyStart.Year == fyEnd.Year ? $"FY {fyStart.Year}" : $"FY {fyStart.Year}-{(fyEnd.Year % 100):D2}";
        return (fyStart, fyEnd, $"{span} ({fyStart.ToString(fmt)} → {fyEnd.ToString(fmt)})");
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
