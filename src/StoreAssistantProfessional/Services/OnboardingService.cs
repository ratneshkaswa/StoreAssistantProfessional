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
    OnboardingStep CurrentStep();
    bool IsComplete();
    string RouteFor(OnboardingStep step);
}

public sealed class OnboardingService : IOnboardingService
{
    private readonly IDbContextFactory<AppDbContext> _dbFactory;

    public OnboardingService(IDbContextFactory<AppDbContext> dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public OnboardingStep CurrentStep()
    {
        using var db = _dbFactory.CreateDbContext();
        if (!db.Firms.Any()) return OnboardingStep.Firm;
        if (!db.TaxRates.Any(t => t.IsActive)) return OnboardingStep.Tax;
        if (!db.Products.Any(p => p.IsActive)) return OnboardingStep.Products;
        if (!db.Vendors.Any(v => v.IsActive)) return OnboardingStep.Vendor;
        return OnboardingStep.Done;
    }

    public bool IsComplete() => CurrentStep() == OnboardingStep.Done;

    public string RouteFor(OnboardingStep step) => step switch
    {
        OnboardingStep.Firm => "/onboarding/firm",
        OnboardingStep.Tax => "/onboarding/tax",
        OnboardingStep.Products => "/onboarding/products",
        OnboardingStep.Vendor => "/onboarding/vendor",
        _ => "/"
    };
}
