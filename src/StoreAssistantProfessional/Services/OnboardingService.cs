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
        if (!await db.Firms.AnyAsync()) return OnboardingStep.Firm;
        if (!await db.TaxRates.AnyAsync()) return OnboardingStep.Tax;
        if (!await db.Products.AnyAsync()) return OnboardingStep.Products;
        if (!await db.Vendors.AnyAsync()) return OnboardingStep.Vendor;
        return OnboardingStep.Done;
    }

    public string RouteFor(OnboardingStep step) => step switch
    {
        OnboardingStep.Firm => "/onboarding/firm",
        OnboardingStep.Tax => "/onboarding/tax",
        OnboardingStep.Products => "/onboarding/products",
        OnboardingStep.Vendor => "/onboarding/vendor",
        _ => "/"
    };
}
