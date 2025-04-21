using Ardalis.GuardClauses;

namespace AdvancedTests.ECommerce.Domain.Entities;

public class Customer
{
    public Customer(int id, string name, bool isPremium)
    {
        Id = Guard.Against.NegativeOrZero(id);
        Name = Guard.Against.NullOrWhiteSpace(name);
        IsPremium = isPremium;
    }

    public int Id { get; }
    public string Name { get; }
    public bool IsPremium { get; }
}