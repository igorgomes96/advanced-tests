using Ardalis.GuardClauses;

namespace AdvancedTests.ECommerce.Domain.Entities;

public class OrderItem
{
    public OrderItem(string name, int quantity, decimal price, decimal discount = 0)
    {
        Name = Guard.Against.NullOrWhiteSpace(name);
        Quantity = Guard.Against.NegativeOrZero(quantity);
        Price = Guard.Against.Negative(price);
        Discount = Guard.Against.OutOfRange(discount, nameof(discount), 0, 1);
    }
    public string Name { get; }
    public int Quantity { get; }
    public decimal Price { get; }
    public decimal Discount { get; }
    public decimal Total => Quantity * Price * (1 - Discount);
}