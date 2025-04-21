using System.Data;
using AdvancedTests.ECommerce.Domain.Entities;
using AdvancedTests.ECommerce.Domain.Repositories;
using Dapper;

namespace AdvancedTests.ECommerce.Infrastructure.Repositories;

public class CustomerRepository(IDbConnection connection) : ICustomerRepository
{
    public async Task<Customer?> GetByIdAsync(int id)
    {
        var customer = await connection.QueryAsync(
            @"select id, name, is_premium from customers where id = @id", new { id });
        var customerData = customer.FirstOrDefault();
        return customerData == null
            ? null
            : new Customer(customerData.id, customerData.name, customerData.is_premium);
    }
}