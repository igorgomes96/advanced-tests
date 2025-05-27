namespace AdvancedTests.ECommerce.Application.UseCases.ListOrders;

public interface IListOrders
{
    Task<IEnumerable<ListOrdersOutput>> ExecuteAsync(ListOrdersInput input, CancellationToken cancellationToken);
}