using AdvancedTests.ECommerce.Domain.Entities;

namespace AdvancedTests.ECommerce.Domain.Repositories;

public interface IOrderRepository
{
    Task<int> AddAsync(Order order);
}