using AdvancedTests.ECommerce.Domain.Entities;

namespace AdvancedTests.ECommerce.Domain.Repositories;

public interface IInventoryRepository
{
    Task<IEnumerable<ProductInventory>> GetByProductNamesAsync(IEnumerable<string> productNames,
        CancellationToken cancellationToken = default);
    Task UpdateAsync(IEnumerable<ProductInventory> inventory, CancellationToken cancellationToken = default);
}