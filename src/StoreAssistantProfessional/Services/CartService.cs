using System.IO;
using System.Text.Json;
using StoreAssistantProfessional.Models;

namespace StoreAssistantProfessional.Services;

public sealed class CartLine
{
    public int ProductId { get; set; }
    public string Description { get; set; } = "";
    public string? Sku { get; set; }
    public string? Barcode { get; set; }
    public string? HsnCode { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Mrp { get; set; }
    public decimal Discount { get; set; }
    public int? TaxRateId { get; set; }
    public decimal TaxPercent { get; set; }

    public decimal Net => decimal.Round((UnitPrice * Quantity) - Discount, 2);
    public decimal TaxAmount => decimal.Round(Net * TaxPercent / 100m, 2);
    public decimal LineTotal => decimal.Round(Net + TaxAmount, 2);
}

public sealed class CartSnapshot
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime At { get; set; } = DateTime.UtcNow;
    public int? CustomerId { get; set; }
    public int? StaffId { get; set; }
    public string? Notes { get; set; }
    public List<CartLine> Lines { get; set; } = new();

    public decimal Subtotal => Lines.Sum(l => l.UnitPrice * l.Quantity);
    public decimal DiscountTotal => Lines.Sum(l => l.Discount);
    public decimal TaxTotal => Lines.Sum(l => l.TaxAmount);
    public decimal Total => Lines.Sum(l => l.LineTotal);
}

public interface ICartService
{
    event Action? Changed;

    CartSnapshot Active { get; }
    IReadOnlyList<CartSnapshot> Held { get; }

    void AddLine(Product product, decimal qty = 1m);
    void IncrementLine(int productId, decimal delta = 1m);
    void RemoveLine(int productId);
    void SetQuantity(int productId, decimal qty);
    void SetUnitPrice(int productId, decimal price);
    void SetLineDiscount(int productId, decimal discount);
    void SetCustomer(int? customerId);
    void SetStaff(int? staffId);
    void SetNotes(string? notes);

    void HoldActive();
    void ResumeHeld(Guid id);
    void DiscardHeld(Guid id);
    void ResetActive();
}

public sealed class CartService : ICartService
{
    private readonly object _lock = new();
    private CartSnapshot _active = new();
    private readonly List<CartSnapshot> _held = new();

    private readonly string _draftPath;
    private readonly string _heldPath;

    public event Action? Changed;

    public CartSnapshot Active { get { lock (_lock) return _active; } }
    public IReadOnlyList<CartSnapshot> Held { get { lock (_lock) return _held.ToList(); } }

    public CartService()
    {
        var dir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "StoreAssistantProfessional");
        Directory.CreateDirectory(dir);
        _draftPath = Path.Combine(dir, "active-cart.json");
        _heldPath  = Path.Combine(dir, "held-bills.json");

        TryLoad();
    }

    private void TryLoad()
    {
        try
        {
            if (File.Exists(_draftPath))
            {
                var json = File.ReadAllText(_draftPath);
                if (!string.IsNullOrWhiteSpace(json))
                    _active = JsonSerializer.Deserialize<CartSnapshot>(json) ?? new CartSnapshot();
            }
            if (File.Exists(_heldPath))
            {
                var json = File.ReadAllText(_heldPath);
                if (!string.IsNullOrWhiteSpace(json))
                {
                    var list = JsonSerializer.Deserialize<List<CartSnapshot>>(json);
                    if (list is not null) _held.AddRange(list);
                }
            }
        }
        catch { /* corrupted draft — start fresh */ }
    }

    private void Persist()
    {
        try
        {
            File.WriteAllText(_draftPath, JsonSerializer.Serialize(_active));
            File.WriteAllText(_heldPath, JsonSerializer.Serialize(_held));
        }
        catch { /* best-effort */ }
        Changed?.Invoke();
    }

    public void AddLine(Product product, decimal qty = 1m)
    {
        lock (_lock)
        {
            var existing = _active.Lines.FirstOrDefault(l => l.ProductId == product.Id);
            if (existing is not null)
            {
                existing.Quantity += qty;
            }
            else
            {
                _active.Lines.Add(new CartLine
                {
                    ProductId = product.Id,
                    Description = product.Name,
                    Sku = product.Sku,
                    Barcode = product.Barcode,
                    HsnCode = product.HsnCode,
                    Quantity = qty,
                    UnitPrice = product.SalePrice > 0 ? product.SalePrice : product.Mrp,
                    Mrp = product.Mrp,
                    TaxRateId = product.TaxRateId,
                    TaxPercent = product.TaxRate?.Rate ?? 0m
                });
            }
        }
        Persist();
    }

    public void IncrementLine(int productId, decimal delta = 1m)
    {
        lock (_lock)
        {
            var l = _active.Lines.FirstOrDefault(x => x.ProductId == productId);
            if (l is null) return;
            l.Quantity = Math.Max(0m, l.Quantity + delta);
            if (l.Quantity == 0m) _active.Lines.Remove(l);
        }
        Persist();
    }

    public void RemoveLine(int productId)
    {
        lock (_lock) _active.Lines.RemoveAll(l => l.ProductId == productId);
        Persist();
    }

    public void SetQuantity(int productId, decimal qty)
    {
        lock (_lock)
        {
            var l = _active.Lines.FirstOrDefault(x => x.ProductId == productId);
            if (l is null) return;
            if (qty <= 0m) _active.Lines.Remove(l);
            else l.Quantity = qty;
        }
        Persist();
    }

    public void SetUnitPrice(int productId, decimal price)
    {
        lock (_lock)
        {
            var l = _active.Lines.FirstOrDefault(x => x.ProductId == productId);
            if (l is null) return;
            l.UnitPrice = Math.Max(0m, price);
        }
        Persist();
    }

    public void SetLineDiscount(int productId, decimal discount)
    {
        lock (_lock)
        {
            var l = _active.Lines.FirstOrDefault(x => x.ProductId == productId);
            if (l is null) return;
            l.Discount = Math.Max(0m, discount);
        }
        Persist();
    }

    public void SetCustomer(int? customerId)
    {
        lock (_lock) _active.CustomerId = customerId;
        Persist();
    }

    public void SetStaff(int? staffId)
    {
        lock (_lock) _active.StaffId = staffId;
        Persist();
    }

    public void SetNotes(string? notes)
    {
        lock (_lock) _active.Notes = notes;
        Persist();
    }

    public void HoldActive()
    {
        lock (_lock)
        {
            if (_active.Lines.Count == 0) return;
            _active.At = DateTime.UtcNow;
            _held.Add(_active);
            _active = new CartSnapshot();
        }
        Persist();
    }

    public void ResumeHeld(Guid id)
    {
        lock (_lock)
        {
            var found = _held.FirstOrDefault(x => x.Id == id);
            if (found is null) return;
            if (_active.Lines.Count > 0) _held.Add(_active);
            _held.Remove(found);
            _active = found;
        }
        Persist();
    }

    public void DiscardHeld(Guid id)
    {
        lock (_lock) _held.RemoveAll(x => x.Id == id);
        Persist();
    }

    public void ResetActive()
    {
        lock (_lock) _active = new CartSnapshot();
        Persist();
    }
}
