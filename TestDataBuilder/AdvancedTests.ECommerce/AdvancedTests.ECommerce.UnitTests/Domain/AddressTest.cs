using AdvancedTests.ECommerce.Domain;
using FluentAssertions;

namespace AdvancedTests.ECommerce.UnitTests.Domain;

public class AddressTest
{
    public Address CreateAddress(
        string street = "Rua 1",
        string city = "São Paulo")
    {
        var state = "SP";
        var zipCode = "98101-123";
        var number = 123;
        return new Address(street, city, state, zipCode, number);
    }
    
    [Fact]
    public void ThrowsExceptionWhenConstructingAndStreetIsNull()
    {
        

        var act = () => CreateAddress(null!);

        act.Should().Throw<ArgumentNullException>();
    }
    
    [Fact]
    public void ThrowsExceptionWhenConstructingAndStreetIsEmpty()
    {
        var street = "";
        var city = "São Paulo";
        var state = "SP";
        var zipCode = "98101-123";
        var number = 123;

        var act = () => new Address(street, city, state, zipCode, number);

        act.Should().Throw<ArgumentException>();
    }
    
    [Fact]
    public void ThrowsExceptionWhenConstructingAndCityIsNull()
    {
        string city = null!;

        var act = () => CreateAddress(city: city);

        act.Should().Throw<ArgumentNullException>();
    }
    
    [Fact]
    public void ThrowsExceptionWhenConstructingAndCityIsEmpty()
    {
        var street = "Rua 1";
        var city = "";
        var state = "SP";
        var zipCode = "98101-123";
        var number = 123;

        var act = () => new Address(street, city, state, zipCode, number);

        act.Should().Throw<ArgumentException>();
    }
}