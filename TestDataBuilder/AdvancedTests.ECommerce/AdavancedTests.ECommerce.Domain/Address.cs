using Ardalis.GuardClauses;

namespace AdvancedTests.ECommerce.Domain;

public class Address
{
    public Address(string street, string city, string state, string zipCode, int number)
    {
        Street = Guard.Against.NullOrWhiteSpace(street);
        City = Guard.Against.NullOrWhiteSpace(city);
        Guard.Against.NullOrWhiteSpace(state);
        State = Guard.Against.LengthOutOfRange(state, 2, 2);
        Guard.Against.NullOrWhiteSpace(zipCode);
        ZipCode = Guard.Against.InvalidFormat(zipCode, nameof(zipCode), @"^\d{5}-?\d{3}$");
        Number = Guard.Against.NegativeOrZero(number);
    }
    
    public string Street { get; }
    public string City { get; }
    public string State { get; }
    public string ZipCode { get; }
    public int Number { get; }
}