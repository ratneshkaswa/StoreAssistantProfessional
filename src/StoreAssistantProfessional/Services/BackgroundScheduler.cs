using Microsoft.EntityFrameworkCore;
using StoreAssistantProfessional.Data;
using StoreAssistantProfessional.Models;

namespace StoreAssistantProfessional.Services;

public interface IBackgroundScheduler : IDisposable
{
}

public sealed class BackgroundScheduler : IBackgroundScheduler
{
    private readonly Timer _timer;
    private readonly IDbContextFactory<AppDbContext> _dbFactory;
    private readonly IAppSettingsService _settings;
    private readonly IBackupService _backup;

    private DateTime? _lastBackupRunDate;
    private DateTime? _lastRecurringRunDate;

    public BackgroundScheduler(
        IDbContextFactory<AppDbContext> dbFactory,
        IAppSettingsService settings,
        IBackupService backup)
    {
        _dbFactory = dbFactory;
        _settings = settings;
        _backup = backup;
        // First tick after 60s, then every 60s.
        _timer = new Timer(_ => _ = TickAsync(), null, TimeSpan.FromSeconds(60), TimeSpan.FromSeconds(60));
    }

    private async Task TickAsync()
    {
        try
        {
            var settings = await _settings.LoadAsync();
            var now = DateTime.Now;
            var today = now.Date;

            await PostDueRecurringAsync(today);
            await ScheduleDailyBackupAsync(settings, now, today);
        }
        catch
        {
            // Background scheduler must never crash.
        }
    }

    private async Task PostDueRecurringAsync(DateTime today)
    {
        // Run at most once per local-day, ignoring app restarts.
        if (_lastRecurringRunDate == today) return;

        await using var db = await _dbFactory.CreateDbContextAsync();
        var due = await db.RecurringExpenses
            .Where(r => r.IsActive && r.AutoPost && r.NextDue != null && r.NextDue <= today)
            .ToListAsync();

        foreach (var r in due)
        {
            db.Expenses.Add(new Expense
            {
                At = DateTime.UtcNow,
                Category = r.Category,
                Description = $"{r.Name} (auto)",
                Amount = r.Amount,
                Source = "Drawer",
                RecurringExpenseId = r.Id
            });
            r.LastPosted = DateTime.UtcNow;
            r.NextDue = ComputeNextDue(r);
        }

        if (due.Count > 0) await db.SaveChangesAsync();

        _lastRecurringRunDate = today;
    }

    private async Task ScheduleDailyBackupAsync(AppSettings settings, DateTime now, DateTime today)
    {
        if (!settings.BackupLocal) return;
        if (!string.Equals(settings.BackupSchedule, "Daily", StringComparison.OrdinalIgnoreCase)) return;

        // Run once after the configured hour:minute, at most once per day.
        if (_lastBackupRunDate == today) return;
        var dueAt = today.AddHours(settings.BackupHour).AddMinutes(settings.BackupMinute);
        if (now < dueAt) return;

        try
        {
            await Task.Run(() => _backup.CreateBackup(settings.BackupRetentionCount));
        }
        catch { /* swallow */ }

        _lastBackupRunDate = today;
    }

    private static DateTime ComputeNextDue(RecurringExpense r)
    {
        var today = DateTime.Today;
        if (r.Frequency == "Yearly" && r.Month is int month)
        {
            var day = Math.Min(r.DayOfMonth, DateTime.DaysInMonth(today.Year, month));
            var thisYear = new DateTime(today.Year, month, day);
            return thisYear > today ? thisYear : thisYear.AddYears(1);
        }
        else
        {
            var day = Math.Min(r.DayOfMonth, DateTime.DaysInMonth(today.Year, today.Month));
            var thisMonth = new DateTime(today.Year, today.Month, day);
            return thisMonth > today ? thisMonth : thisMonth.AddMonths(1);
        }
    }

    public void Dispose() => _timer.Dispose();
}
