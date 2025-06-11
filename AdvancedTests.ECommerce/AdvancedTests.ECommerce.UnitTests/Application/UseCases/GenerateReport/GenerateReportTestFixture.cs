using UseCase = AdvancedTests.ECommerce.Application.UseCases.GenerateReport;

namespace AdvancedTests.ECommerce.UnitTests.Application.UseCases.GenerateReport;

public class GenerateReportTestFixture
{
    public UseCase.GenerateReport UseCase { get; } = new();
}