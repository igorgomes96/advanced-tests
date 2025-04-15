using AdvancedTests.ECommerce.Domain;
using AdvancedTests.ECommerce.UnitTests.DataBuilders;
using FluentAssertions;

namespace AdvancedTests.ECommerce.UnitTests.Domain;

public class AddressTest
{
    [Fact]
    public void ThrowsExceptionWhenConstructingAndStreetIsNull()
    {
        var act = () => new AddressDataBuilder().WithStreet(null!).Build();
        act.Should().Throw<ArgumentNullException>();
    }
    
    [Fact]
    public void ThrowsExceptionWhenConstructingAndStreetIsEmpty()
    {
        var act = () => new AddressDataBuilder().WithStreet("").Build();

        act.Should().Throw<ArgumentException>();
    }
    
    [Fact]
    public void ThrowsExceptionWhenConstructingAndCityIsNull()
    {
        var act = () => new AddressDataBuilder().WithCity(null!).Build();

        act.Should().Throw<ArgumentNullException>();
    }
    
    [Fact]
    public void ThrowsExceptionWhenConstructingAndCityIsEmpty()
    {
        var act = () => new AddressDataBuilder().WithCity("").Build();

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void CreateAddressWhenArgumentsAreValid()
    {
        var address = new AddressDataBuilder().Build();
        address.Should().NotBeNull();
    }
}