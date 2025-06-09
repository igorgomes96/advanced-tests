using System.Data;
using AdvancedTests.ECommerce.Domain.Entities;
using AdvancedTests.ECommerce.Domain.Repositories;
using Dapper;

namespace AdvancedTests.ECommerce.Infrastructure.Repositories;

public class InventoryRepository(IDbConnection connection) : IInventoryRepository
{
    public async Task<IEnumerable<ProductInventory>> GetByProductNamesAsync(IEnumerable<string> productNames,
        CancellationToken cancellationToken = default)
    {
        const string query = 
            """
            SELECT product_name as ProductName, quantity
            FROM inventory WHERE product_name IN @productNames
            FOR UPDATE
            """;
        return await connection.QueryAsync<ProductInventory>(query, new { productNames });
    }

    public async Task UpdateAsync(IEnumerable<ProductInventory> inventory,
        CancellationToken cancellationToken = default)
    {
        const string query = "UPDATE inventory SET quantity = @Quantity WHERE product_name = @ProductName";
        await connection.ExecuteAsync(query, inventory.Select(x => new { x.ProductName, x.Quantity }));
    }
}