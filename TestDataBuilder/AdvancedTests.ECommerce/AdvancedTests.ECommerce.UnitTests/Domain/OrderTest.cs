using AdvancedTests.ECommerce.Domain;
using FluentAssertions;

namespace AdvancedTests.ECommerce.UnitTests.Domain;

public class OrderTest
{
    public Address CreateAddress()
    {
        var street = "Rua 1";
        var city = "São Paulo";
        var state = "SP";
        var zipCode = "98101-123";
        var number = 123;
        return new Address(street, city, state, zipCode, number);
    }
    [Fact]
    public void ThrowsAnExceptionWhenConstructionWithCustomerNull()
    {
        var street = "Rua 1";
        var city = "São Paulo";
        var state = "SP";
        var zipCode = "98101-123";
        var number = 123;
        var address = new Address(street, city, state, zipCode, number);
        Customer customer = null!;
        var itemName = "Item 1";
        var quantity = 2;
        var price = 10.0m;
        var item = new OrderItem(itemName, quantity, price);
        
        var act = () => new Order(customer, address, [item]);
        
        act.Should().Throw<ArgumentNullException>();
    }
}