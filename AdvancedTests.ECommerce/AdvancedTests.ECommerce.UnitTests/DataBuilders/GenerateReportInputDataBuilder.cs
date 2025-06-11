using AdvancedTests.ECommerce.Application.UseCases.GenerateReport;
using AdvancedTests.ECommerce.Domain.Entities;
using Bogus;
using static AdvancedTests.ECommerce.UnitTests.DataBuilders.GenerateReportItemInputDataBuilder;

namespace AdvancedTests.ECommerce.UnitTests.DataBuilders;

public class GenerateReportInputDataBuilder
{
    private int _customerId;
    private string _customerName;
    private string _street;
    private string _city;
    private string _state;
    private string _zipCode;
    private int _number;
    private List<GenerateReportItemInput> _items;
    private bool _defaultItems = true;

    public GenerateReportInputDataBuilder()
    {
        var faker = new Faker("pt_BR");
        _customerId = faker.Random.Int(1, 1000);
        _street = faker.Address.StreetName();
        _city = faker.Address.City();
        _state = faker.Address.StateAbbr();
        _zipCode = faker.Address.ZipCode();
        _number = faker.Random.Int(1, 1000);
        _items = new List<GenerateReportItemInput>
        {
            AnItemInput().Build(),
            AnItemInput().Build()
        };
    }
    
    public static GenerateReportInputDataBuilder AGenerateReportInput() => new();
    
    public GenerateReportInputDataBuilder FromCustomer(Customer customer)
    {
        _customerId = customer.Id;
        _customerName = customer.Name;
        return this;
    }
    
    public GenerateReportInputDataBuilder WithItem(int quantity, decimal price)
    {
        if (_defaultItems)
        {
            _items.Clear();
            _defaultItems = false;
        }
        _items.Add(new GenerateReportItemInputDataBuilder().WithQuantity(quantity).WithPrice(price).Build());
        return this;
    }
    
    public GenerateReportInput Build()
    {
        return new GenerateReportInput(
            new GenerateReportCustomerInput(_customerId, _customerName),
            new GenerateReportAddressInput(_street, _city, _state, _zipCode, _number),
            _items);
    }
}