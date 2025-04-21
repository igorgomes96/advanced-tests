using AdvancedTests.ECommerce.Application.UseCases.CreateOrder;

namespace AdvancedTests.ECommerce.IntegrationTests.DataBuilders;

public class CreateOrderItemInputDataBuilder
{
    private string _name;
    private int _quantity;
    private decimal _price;
    
    public CreateOrderItemInputDataBuilder()
    {
        var faker = new Bogus.Faker("pt_BR");
        _name = faker.Commerce.ProductName();
        _quantity = faker.Random.Int(1, 10);
        _price = decimal.Parse(faker.Commerce.Price());
    }
    
    public static CreateOrderItemInputDataBuilder AnItemInput() => new();
    
    public CreateOrderItemInput Build()
    {
        return new CreateOrderItemInput(_name, _quantity, _price);
    }
}