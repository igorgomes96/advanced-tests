using AdvancedTests.ECommerce.Domain.Entities;

namespace AdvancedTests.ECommerce.Application.UseCases.GenerateReport;

public record GenerateReportInput(
    GenerateReportCustomerInput Customer,
    GenerateReportAddressInput Address,
    IEnumerable<GenerateReportItemInput> Items);

public record GenerateReportCustomerInput(int CustomerId, string Name)
{
    public Customer ToCustomer()
    {
        return new Customer(CustomerId, Name, false);
    }
}
    
public record GenerateReportAddressInput(
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

public record GenerateReportItemInput(
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