using AdvancedTests.ECommerce.Domain.Entities;

namespace AdvancedTests.ECommerce.Domain.Repositories;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(int id);
}