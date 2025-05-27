using AdvancedTests.ECommerce.Application.UseCases.ListOrders;

namespace AdvancedTests.ECommerce.Application.Queries;

public interface IOrderQueries
{
    Task<IEnumerable<ListOrdersOutput>> ListOrdersAsync(ListOrdersInput input, CancellationToken cancellationToken);
}