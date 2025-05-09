using System.Data;
using AdvancedTests.ECommerce.Application.Queries;
using AdvancedTests.ECommerce.Application.UseCases.ListOrders;
using Dapper;

namespace AdvancedTests.ECommerce.Infrastructure.Queries;

public class OrderQueries(IDbConnection connection) : IOrderQueries
{
    public async Task<IEnumerable<ListOrdersOutput>> ListOrdersAsync(ListOrdersInput input,
        CancellationToken cancellationToken)
    {
        var sql = @"
            select
                o.id as Id,
                c.name as CustomerName,
                o.amount as Amount,
                o.status as Status
            from orders o inner join customers c on o.customer_id = c.id
            where o.customer_id = @CustomerId
            order by id desc;";

        return await connection.QueryAsync<ListOrdersOutput>(sql, new { input.CustomerId });
    }
}