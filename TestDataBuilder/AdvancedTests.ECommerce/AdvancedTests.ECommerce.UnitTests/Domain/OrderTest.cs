using AdvancedTests.ECommerce.Domain;
using AdvancedTests.ECommerce.UnitTests.DataBuilders;
using FluentAssertions;

namespace AdvancedTests.ECommerce.UnitTests.Domain;

public class OrderTest
{
    [Fact]
    public void ThrowsAnExceptionWhenConstructionWithCustomerNull()
    {
        var act = () => new OrderDataBuilder().WithCustomer(null!).Build();
        act.Should().Throw<ArgumentNullException>();
    }
    
    [Fact]
    public void CreateOrderWhenArgumentsAreValid()
    {
        var order = new OrderDataBuilder().Build();
        order.Should().NotBeNull();
    }
}