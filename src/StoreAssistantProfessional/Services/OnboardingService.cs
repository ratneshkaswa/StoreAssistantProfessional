using Microsoft.EntityFrameworkCore;
using StoreAssistantProfessional.Data;

namespace StoreAssistantProfessional.Services;

public enum OnboardingStep
{
    Firm,
    Tax,
    Products,
    Vendor,
    Done
}

public interface IOnboardingService
{
    Task<OnboardingStep> CurrentStepAsync();
    string RouteFor(OnboardingStep step);
}

public sealed class OnboardingService : IOnboardingService
{
    private readonly IDbContextFactory<AppDbContext> _dbFactory;

    public OnboardingService(IDbContextFactory<AppDbContext> dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public async Task<OnboardingStep> CurrentStepAsync()
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        if (!await db.Firms.AnyAsync(f => !string.IsNullOrEmpty(f.Name))) return OnboardingStep.Firm;
        if (!await db.TaxRates.AnyAsync(t => t.IsActive)) return OnboardingStep.Tax;
        if (!await db.Products.AnyAsync(p => p.IsActive)) return OnboardingStep.Products;
        if (!await db.Vendors.AnyAsync(v => v.IsActive)) return OnboardingStep.Vendor;
        return OnboardingStep.Done;
    }

    public string RouteFor(OnboardingStep step) => RouteForStep(step) ?? "/";

    public static string? RouteForStep(OnboardingStep step) => step switch
    {
        OnboardingStep.Firm => "/onboarding/firm",
        OnboardingStep.Tax => "/onboarding/tax",
        OnboardingStep.Products => "/onboarding/products",
        OnboardingStep.Vendor => "/onboarding/vendor",
        _ => null,
    };
}
