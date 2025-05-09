using AdvancedTests.ECommerce.Application.Queries;

namespace AdvancedTests.ECommerce.Application.UseCases.ListOrders;

public class ListOrders(IOrderQueries queries) : IListOrders
{
    public async Task<IEnumerable<ListOrdersOutput>> ExecuteAsync(ListOrdersInput input, CancellationToken cancellationToken)
        => await queries.ListOrdersAsync(input, cancellationToken);
}