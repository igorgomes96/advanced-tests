using AdvancedTests.ECommerce.Application.UseCases.CreateOrder;
using AdvancedTests.ECommerce.Application.UseCases.GenerateReport;

namespace AdvancedTests.ECommerce.UnitTests.DataBuilders;

public class GenerateReportItemInputDataBuilder
{
    private string _name;
    private int _quantity;
    private decimal _price;
    
    public GenerateReportItemInputDataBuilder()
    {
        var faker = new Bogus.Faker("pt_BR");
        _name = faker.Commerce.ProductName();
        _quantity = faker.Random.Int(1, 10);
        _price = decimal.Parse(faker.Commerce.Price());
    }
    
    public static GenerateReportItemInputDataBuilder AnItemInput() => new();

    
    public GenerateReportItemInputDataBuilder WithQuantity(int quantity)
    {
        _quantity = quantity;
        return this;
    }
    
    public GenerateReportItemInputDataBuilder WithPrice(decimal price)
    {
        _price = price;
        return this;
    }
    
    public GenerateReportItemInput Build()
    {
        return new GenerateReportItemInput(_name, _quantity, _price);
    }
}