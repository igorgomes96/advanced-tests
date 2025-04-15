using AdvancedTests.ECommerce.Domain;
using AdvancedTests.ECommerce.UnitTests.DataBuilders;
using FluentAssertions;
using static AdvancedTests.ECommerce.UnitTests.DataBuilders.CustomerDataBuilder;
using static AdvancedTests.ECommerce.UnitTests.DataBuilders.OrderDataBuilder;

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
    
    [Fact]
    public void Give10PercentDiscountForPremiumCustomer()
    {
        var anOrder = AnOrder()
            .With(APremiumCustomer())
            .WithItem(quantity: 2, price: 10)
            .WithItem(quantity: 4, price: 20)
            .Build();
        
        anOrder.Amount.Should().Be(90);
    }
}