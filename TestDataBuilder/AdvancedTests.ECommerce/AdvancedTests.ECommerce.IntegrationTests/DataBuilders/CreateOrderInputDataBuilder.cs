using AdvancedTests.ECommerce.Application.UseCases.CreateOrder;
using Bogus;
using static AdvancedTests.ECommerce.IntegrationTests.DataBuilders.CreateOrderItemInputDataBuilder;

namespace AdvancedTests.ECommerce.IntegrationTests.DataBuilders;

public class CreateOrderInputDataBuilder
{
    private int _customerId;
    private string _street;
    private string _city;
    private string _state;
    private string _zipCode;
    private int _number;
    private List<CreateOrderItemInput> _items;

    public CreateOrderInputDataBuilder()
    {
        var faker = new Faker("pt_BR");
        _customerId = faker.Random.Int(1, 1000);
        _street = faker.Address.StreetName();
        _city = faker.Address.City();
        _state = faker.Address.StateAbbr();
        _zipCode = faker.Address.ZipCode();
        _number = faker.Random.Int(1, 1000);
        _items = new List<CreateOrderItemInput>
        {
            AnItemInput().Build(),
            AnItemInput().Build()
        };
    }
    
    public static CreateOrderInputDataBuilder AnOrderInput() => new();
    
    public CreateOrderInputDataBuilder FromCustomerId(int customerId)
    {
        _customerId = customerId;
        return this;
    }
    
    public CreateOrderInput Build()
    {
        return new CreateOrderInput(
            _customerId,
            new CreateOrderAddressInput(_street, _city, _state, _zipCode, _number),
            _items);
    }
}