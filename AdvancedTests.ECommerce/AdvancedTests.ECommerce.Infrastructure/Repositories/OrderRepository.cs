using System.Data;
using AdvancedTests.ECommerce.Domain.Entities;
using AdvancedTests.ECommerce.Domain.Repositories;
using Dapper;

namespace AdvancedTests.ECommerce.Infrastructure.Repositories;

public class OrderRepository(IDbConnection connection) : IOrderRepository
{
    public async Task<int> AddAsync(Order order)
    {
        var sql = @"
            insert into orders (customer_id, state, city, street, zip_code, number, amount, status)
            values (@CustomerId, @State, @City, @Street, @ZipCode, @Number, @Amount, @Status);
            select LAST_INSERT_ID();";
        var parameters = new
        {
            CustomerId = order.Customer.Id,
            order.Address.State,
            order.Address.City,
            order.Address.Street,
            order.Address.ZipCode,
            order.Address.Number,
            order.Amount,
            order.Status
        };

        return await connection.ExecuteScalarAsync<int>(sql, parameters);
    }
}