using FluentAssertions;
using static AdvancedTests.ECommerce.UnitTests.DataBuilders.GenerateReportInputDataBuilder;
using static AdvancedTests.ECommerce.UnitTests.DataBuilders.CustomerDataBuilder;
namespace AdvancedTests.ECommerce.UnitTests.Application.UseCases.GenerateReport;

public class GenerateReportTest(GenerateReportTestFixture fixture) : IClassFixture<GenerateReportTestFixture>
{
    [Fact]
    public void ReturnsValidReportWhenGenerateReportUseCaseIsCalled()
    {
        const int ordersCount = 500;
        const decimal pricePerOrder = 10m;
        const decimal totalAmount = pricePerOrder * ordersCount;
        const int customersCount = 5;
        const decimal amountPerCustomer = totalAmount / customersCount;
        
        var customers = Enumerable.Range(0, customersCount)
            .Select(i => ARegularCustomer().WithName("Customer " + i).Build())
            .ToArray();

        var input = Enumerable.Range(0, ordersCount)
            .Select(i =>  
                AGenerateReportInput()
                    .FromCustomer(customers[i % 5])
                    .WithItem(1, pricePerOrder)
                    .Build())
            .ToArray();
        
        var report = fixture.UseCase.Execute(input);
        
        report.Total.Should().Be(totalAmount);
        report.AmountPerCustomer
            .Should()
            .HaveCount(customersCount)
            .And
            .AllSatisfy(item => item.Value.Should().Be(amountPerCustomer));
    }
}