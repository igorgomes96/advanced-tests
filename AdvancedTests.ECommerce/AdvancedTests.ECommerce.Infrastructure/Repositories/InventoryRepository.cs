using System.Data;
using AdvancedTests.ECommerce.Domain.Entities;
using AdvancedTests.ECommerce.Domain.Repositories;
using AdvancedTests.ECommerce.Infrastructure.Exceptions;
using Dapper;

namespace AdvancedTests.ECommerce.Infrastructure.Repositories;

public class InventoryRepository(IDbConnection connection) : IInventoryRepository
{
    public async Task<IEnumerable<ProductInventory>> GetByProductNamesAsync(IEnumerable<string> productNames,
        CancellationToken cancellationToken = default)
    {
        const string query =
            """
            SELECT product_name as ProductName, quantity, version_id as VersionId
            FROM inventory WHERE product_name IN @productNames
            """;
        return await connection.QueryAsync<ProductInventory>(query, new { productNames });
    }

    public async Task UpdateAsync(IEnumerable<ProductInventory> inventory,
        CancellationToken cancellationToken = default)
    {
        const string query =
            """
            UPDATE inventory SET quantity = @Quantity, version_id = @VersionId + 1
            WHERE product_name = @ProductName and version_id = @VersionId
            """;
        var affectedRows = await connection.ExecuteAsync(query,
            inventory.Select(x => new { x.ProductName, x.Quantity, x.VersionId }));
        if (affectedRows != inventory.Count())
        {
            throw new ConcurrencyException("Some inventory items were not updated due to concurrency issues.");
        }
    }
}