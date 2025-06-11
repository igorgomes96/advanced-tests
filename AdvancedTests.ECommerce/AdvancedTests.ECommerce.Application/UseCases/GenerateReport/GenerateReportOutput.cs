namespace AdvancedTests.ECommerce.Application.UseCases.GenerateReport;

public record GenerateReportOutput(Dictionary<string, decimal> AmountPerCustomer, decimal Total);