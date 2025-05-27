using AdvancedTests.ECommerce.Domain;
using AdvancedTests.ECommerce.UnitTests.DataBuilders;
using FluentAssertions;

namespace AdvancedTests.ECommerce.UnitTests.Domain;

public class AddressTest
{
    [Theory]
    [InlineData(null, "Value cannot be null. (Parameter 'street')")]
    [InlineData("", "Required input street was empty. (Parameter 'street')")]
    [InlineData(" ", "Required input street was empty. (Parameter 'street')")]
    [InlineData("    ", "Required input street was empty. (Parameter 'street')")]
    public void ThrowsExceptionWhenConstructingAndStreetIsInvalid(string street, string errorMessage)
    {
        var act = () => new AddressDataBuilder().WithStreet(street).Build();
        act.Should().Throw<ArgumentException>()
            .WithMessage(errorMessage);
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
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("A")]
    [InlineData("ABC")]
    public void ThrowsExceptionWhenConstructingAndStateIsInvalid(string? state)
    {
        var act = () => new AddressDataBuilder().WithState(state!).Build();
        act.Should().Throw<ArgumentException>();
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("12345-4565")]
    public void ThrowsExceptionWhenConstructingAndZipCodeIsInvalid(string? zipCode)
    {
        var act = () => new AddressDataBuilder().WithZipCode(zipCode!).Build();
        act.Should().Throw<ArgumentException>();
    }
}