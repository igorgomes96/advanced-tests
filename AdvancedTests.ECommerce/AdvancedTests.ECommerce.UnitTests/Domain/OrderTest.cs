using AdvancedTests.ECommerce.Domain.Entities;
using AdvancedTests.ECommerce.UnitTests.DataBuilders;
using FluentAssertions;
using static AdvancedTests.ECommerce.UnitTests.DataBuilders.CustomerDataBuilder;
using static AdvancedTests.ECommerce.UnitTests.DataBuilders.OrderDataBuilder;
using static AdvancedTests.ECommerce.UnitTests.DataBuilders.OrderItemDataBuilder;

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
            .From(APremiumCustomer())
            .WithItem(quantity: 2, price: 1000)
            .WithItem(quantity: 4, price: 2000)
            .Build();
        
        anOrder.Amount.Should().Be(9000);
    }
    
    [Fact]
    public void DontGiveDiscountWhenSubtotalIsEqualTo1000()
    {
        var anOrder = AnOrder()
            .From(APremiumCustomer())
            .WithItem(quantity: 2, price: 100)
            .WithItem(quantity: 4, price: 200)
            .Build();
        
        anOrder.Amount.Should().Be(1000);
    }

    [Fact]
    public void CalculateAmountWithDiscountForMultipleItemsOfSameProduct()
    {
        var book = AnOrderItem()
            .WithName("Refactoring")
            .WithPrice(100)
            .WithQuantity(1);
        
        var bookWithSmallerDiscount = book
            .WithDiscount(0.1m)
            .Build(); 
        
        var bookWithGreaterDiscount = book
            .WithDiscount(0.2m)
            .Build();

        var order = AnOrder()
            .From(ARegularCustomer())
            .WithItems(bookWithSmallerDiscount, bookWithGreaterDiscount)
            .Build();
        
        order.Amount.Should().Be(170);
    }
    
    [Fact]
    public void ThrowsAnExceptionWhenTryingToCancelADeliveredOrder() 
    {
        var order = AnOrder()
            .WithStatus(OrderStatus.Delivered)
            .Build();
        
        var act = () => order.Cancel();
        
        act.Should().Throw<InvalidOperationException>();
    }
    
    [Theory]
    [InlineData(OrderStatus.Created)]
    [InlineData(OrderStatus.Confirmed)]
    [InlineData(OrderStatus.Paid)]
    [InlineData(OrderStatus.Delivered)]
    [InlineData(OrderStatus.Cancelled)]
    public void ThrowsAnExceptionWhenTryingToChangeToDeliveredAnOrderWithStatusDifferentFromShipped(OrderStatus status) 
    {
        var order = AnOrder()
            .WithStatus(status)
            .Build();
        
        var act = () => order.Deliver();
        
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Somente itens no status 'Enviado' podem ser entregues.");
    }

    [Fact]
    public void AddItemToOrderWhenStatusIsCreated()
    {
        var order = AnOrder()
            .WithStatus(OrderStatus.Created)
            .WithItem(1, 10)
            .Build();
        
        var item = AnOrderItem()
            .WithName("Refactoring")
            .WithPrice(100)
            .WithQuantity(1)
            .Build();
        
        order.AddItem(item);
        
        order.Items.Should().Contain(item);
        order.Amount.Should().Be(110);
    }
    
    [Fact]
    public void ThrowsExceptionWhenAddItemToOrderButStatusDoesntAllowIt()
    {
        var order = AnOrder()
            .WithStatus(OrderStatus.Paid)
            .Build();
        
        var item = AnOrderItem().Build();
        
        var act = () => order.AddItem(item);
        
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Item não pode ser adicionado ao pedido.");
    }
}