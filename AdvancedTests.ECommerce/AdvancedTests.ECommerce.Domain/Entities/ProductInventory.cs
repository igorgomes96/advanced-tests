using Ardalis.GuardClauses;

namespace AdvancedTests.ECommerce.Domain.Entities;

public class ProductInventory(string productName, int quantity)
{
    public string ProductName { get; } = Guard.Against.NullOrWhiteSpace(productName);
    public int Quantity { get; private set; } = Guard.Against.Negative(quantity, nameof(quantity));
    
    public void Update(int quantity)
    {
        Guard.Against.Negative(quantity);
        Quantity = quantity;
    }
}