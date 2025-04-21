using AdvancedTests.ECommerce.Domain.Entities;

namespace AdvancedTests.ECommerce.Application.UseCases.CreateOrder;

public record CreateOrderInput(
    int CustomerId,
    CreateOrderAddressInput Address,
    IEnumerable<CreateOrderItemInput> Items);

public record CreateOrderAddressInput(
    string Street,
    string City,
    string State,
    string ZipCode,
    int Number
)
{
    public Address ToAddress()
    {
        return new Address(Street, City, State, ZipCode, Number);
    }
}

public record CreateOrderItemInput(
    string Name,
    int Quantity,
    decimal Price
)
{
    public OrderItem ToOrderItem()
    {
        return new OrderItem(Name, Quantity, Price);
    }
}