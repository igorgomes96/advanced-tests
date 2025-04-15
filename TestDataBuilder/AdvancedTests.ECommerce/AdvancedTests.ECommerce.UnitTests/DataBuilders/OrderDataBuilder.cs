using AdvancedTests.ECommerce.Domain;

namespace AdvancedTests.ECommerce.UnitTests.DataBuilders;

public class OrderDataBuilder
{
    private Customer _customer;
    private Address _address;
    private List<OrderItem> _items;

    public OrderDataBuilder()
    {
        _customer = new CustomerDataBuilder().Build();
        _address = new AddressDataBuilder().Build();
        _items = new List<OrderItem>
        {
            new OrderItemDataBuilder().Build(),
            new OrderItemDataBuilder().Build()
        };
    }

    public OrderDataBuilder WithCustomer(Customer customer)
    {
        _customer = customer;
        return this;
    }

    public OrderDataBuilder WithAddress(Address address)
    {
        _address = address;
        return this;
    }

    public OrderDataBuilder WithItems(List<OrderItem> items)
    {
        _items = items;
        return this;
    }

    public Order Build()
    {
        return new Order(_customer, _address, _items);
    }
}