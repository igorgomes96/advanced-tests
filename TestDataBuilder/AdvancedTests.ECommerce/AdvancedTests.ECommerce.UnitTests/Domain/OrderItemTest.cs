using FluentAssertions;
using static AdvancedTests.ECommerce.UnitTests.DataBuilders.OrderItemDataBuilder;

namespace AdvancedTests.ECommerce.UnitTests.Domain;

public class OrderItemTest
{
    [Theory]
    [InlineData(-0.01)]
    [InlineData(1.01)]
    public void ThrowsArgumentOutOfRangeExceptionWhenDiscountIsOutOfRange(decimal discount)
    {
        var act = () => AnOrderItem().WithDiscount(discount).Build();
        act.Should().Throw<ArgumentOutOfRangeException>();
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(0.5)]
    [InlineData(1)]
    public void CreateOrderItemWhenDiscountIsValid(decimal discount)
    {
        var orderItem = AnOrderItem().WithDiscount(discount).Build();
        orderItem.Should().NotBeNull();
        orderItem.Discount.Should().Be(discount);
    }
}