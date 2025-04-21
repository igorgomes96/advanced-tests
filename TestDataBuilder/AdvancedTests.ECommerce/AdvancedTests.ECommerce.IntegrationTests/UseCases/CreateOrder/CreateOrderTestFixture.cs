using System.Data;
using AdvancedTests.ECommerce.Domain.Entities;
using AdvancedTests.ECommerce.Infrastructure.Repositories;
using AdvancedTests.ECommerce.IntegrationTests.Models;
using Dapper;
using MySqlConnector;
using Testcontainers.MySql;
using UseCase = AdvancedTests.ECommerce.Application.UseCases.CreateOrder;

namespace AdvancedTests.ECommerce.IntegrationTests.UseCases.CreateOrder;

public class CreateOrderTestFixture : IDisposable
{
    private static readonly MySqlContainer MySqlDbContainer =
        new MySqlBuilder()
            .WithImage("public.ecr.aws/lts/mysql:latest")
            .WithDatabase("ecommerce")
            .Build();
    
    public IDbConnection? DbConnection { get; private set; }

    public CreateOrderTestFixture()
    {
        MySqlDbContainer.StartAsync().Wait();
        DbConnection = new MySqlConnection(MySqlDbContainer.GetConnectionString());
        DbConnection!.Execute(
            """
          create table if not exists customers (
              id int not null primary key,
              name varchar(255) not null,
              is_premium boolean not null
          );

          create table if not exists orders (
              id int not null primary key auto_increment,
              customer_id int not null,
              status int not null,
              amount decimal(10, 2) not null,
              street varchar(255) not null,
              city varchar(255) not null,
              state varchar(255) not null,
              zip_code varchar(255) not null,
              number varchar(6) not null,
              foreign key (customer_id) references customers(id)
          );
          """);
    }
    
    public async Task Insert(Customer customer)
    {
        var sql = """
                      insert into customers (id, name, is_premium)
                      values (@Id, @Name, @IsPremium);
                  """;
        await DbConnection!.ExecuteAsync(sql, new
        {
            customer.Id,
            customer.Name,
            customer.IsPremium
        });
    }

    public UseCase.CreateOrder GetUseCase()
    {
        DbConnection?.Close();
        DbConnection = new MySqlConnection(MySqlDbContainer.GetConnectionString());
        var customerRepository = new CustomerRepository(DbConnection);
        var orderRepository = new OrderRepository(DbConnection);
        return new UseCase.CreateOrder(customerRepository, orderRepository);
    }
    
    public async Task<IEnumerable<OrderModel>> GetInsertedOrders()
    {
        var sql = """
                      select 
                          id as Id,
                          customer_id as CustomerId,
                          amount as Amount,
                          status as Status,
                          state as State,
                          city as City,
                          zip_code as ZipCode,
                          number as Number,
                          street as Street
                      from orders;
                  """;
        return await DbConnection!.QueryAsync<OrderModel>(sql);
    }
    
    public void CloseConnection()
    {
        DbConnection?.Close();
        DbConnection = null;
    }
    
    public void CleanUpDatabase()
    {
        DbConnection!.Execute("""
                                  delete from orders;
                                  delete from customers;
                              """);
    }
    
    public void Dispose()
    {
        DbConnection?.Dispose();
        MySqlDbContainer.DisposeAsync()
            .GetAwaiter().GetResult();
    }
}