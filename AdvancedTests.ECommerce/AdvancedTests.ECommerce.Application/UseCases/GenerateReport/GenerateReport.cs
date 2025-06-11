using System.Collections.Concurrent;
using AdvancedTests.ECommerce.Domain.Entities;

namespace AdvancedTests.ECommerce.Application.UseCases.GenerateReport;

public class GenerateReport : IGenerateReport
{
    private readonly object _lock = new object();

    public GenerateReportOutput Execute(IEnumerable<GenerateReportInput> input)
    {
        var totalAmount = 0m;
        var amountPerCustomer = new ConcurrentDictionary<string, decimal>();
        Parallel.ForEach(input, (item, token) =>
        {
            // foreach (var item in input)
            var customer = item.Customer.ToCustomer();
            var order = new Order(customer, item.Address.ToAddress(),
                item.Items.Select(i => i.ToOrderItem()).ToList());
            amountPerCustomer.AddOrUpdate(customer.Name, order.Amount, (_, oldValue) => oldValue + order.Amount);
            lock (_lock)
            {
                totalAmount += order.Amount;
            }
        });

        return new GenerateReportOutput(amountPerCustomer.ToDictionary(), totalAmount);
    }
}