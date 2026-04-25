using StoreAssistantProfessional.Models;

namespace StoreAssistantProfessional.Services;

public static class FinancialYear
{
    public static (DateTime start, DateTime end) Range(Firm firm, DateTime today)
    {
        var month = Math.Clamp(firm.FyStartMonth, 1, 12);
        var day = Math.Clamp(firm.FyStartDay, 1, DateTime.DaysInMonth(today.Year, month));
        var startThisYear = new DateTime(today.Year, month, day);
        var start = today >= startThisYear ? startThisYear : startThisYear.AddYears(-1);
        var end = start.AddYears(1).AddDays(-1);
        return (start, end);
    }

    public static string Label(Firm? firm, DateTime today)
    {
        if (firm is null) return $"FY {today.Year}";
        var (start, end) = Range(firm, today);
        return start.Year == end.Year
            ? $"FY {start.Year}"
            : $"FY {start.Year}-{(end.Year % 100):D2}";
    }
}
