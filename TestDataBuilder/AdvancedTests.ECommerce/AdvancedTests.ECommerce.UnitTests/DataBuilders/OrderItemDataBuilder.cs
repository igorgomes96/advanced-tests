using AdvancedTests.ECommerce.Domain.Entities;
using Bogus;

namespace AdvancedTests.ECommerce.UnitTests.DataBuilders;

public class OrderItemDataBuilder
{
    private string _name;
    private int _quantity;
    private decimal _price;
    private decimal _discount = 0;

    public OrderItemDataBuilder()
    {
        var faker = new Faker("pt_BR");
        _name = faker.Commerce.ProductName();
        _quantity = faker.Random.Int(1, 10);
        _price = faker.Random.Decimal(1, 100);
    }
    
    public static OrderItemDataBuilder AnOrderItem() => new();

    public OrderItemDataBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public OrderItemDataBuilder WithQuantity(int quantity)
    {
        _quantity = quantity;
        return this;
    }

    public OrderItemDataBuilder WithPrice(decimal price)
    {
        _price = price;
        return this;
    }
    
    public OrderItemDataBuilder WithDiscount(decimal discount)
    {
        _discount = discount;
        return this;
    }

    public OrderItem Build()
    {
        return new OrderItem(_name, _quantity, _price, _discount);
    }
}