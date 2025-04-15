using Ardalis.GuardClauses;

namespace AdvancedTests.ECommerce.Domain;

public class OrderItem
{
    public OrderItem(string name, int quantity, decimal price)
    {
        Name = Guard.Against.NullOrWhiteSpace(name);
        Quantity = Guard.Against.NegativeOrZero(quantity);
        Price = Guard.Against.Negative(price);
    }
    public string Name { get; }
    public int Quantity { get; }
    public decimal Price { get; }
    public decimal Total => Quantity * Price;
}