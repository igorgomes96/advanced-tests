namespace AdvancedTests.ECommerce.Application.UseCases.GenerateReport;

public interface IGenerateReport
{
    GenerateReportOutput Execute(IEnumerable<GenerateReportInput> input);
}