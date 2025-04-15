using AdvancedTests.ECommerce.Domain;

namespace AdvancedTests.ECommerce.UnitTests.DataBuilders;

public class OrderDataBuilder
{
    private Customer _customer;
    private Address _address;
    private List<OrderItem> _items;
    private bool _defaultItems = true;

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
    
    public static OrderDataBuilder AnOrder() => new OrderDataBuilder();

    public OrderDataBuilder WithCustomer(Customer customer)
    {
        _customer = customer;
        return this;
    }

    public OrderDataBuilder From(CustomerDataBuilder customerDataBuilder)
    {
        _customer = customerDataBuilder.Build();
        return this;
    }

    public OrderDataBuilder WithAddress(Address address)
    {
        _address = address;
        return this;
    }

    public OrderDataBuilder WithItems(params OrderItem[] items)
    {
        _defaultItems = false;
        _items.Clear();
        _items.AddRange(items);
        return this;
    }

    public OrderDataBuilder WithItem(int quantity, decimal price)
    {
        if (_defaultItems)
        {
            _items.Clear();
            _defaultItems = false;
        }
        _items.Add(new OrderItemDataBuilder().WithQuantity(quantity).WithPrice(price).Build());
        return this;
    }

    public Order Build()
    {
        return new Order(_customer, _address, _items);
    }
}