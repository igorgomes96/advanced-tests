using System.Data;
using AdvancedTests.ECommerce.Domain.Entities;
using AdvancedTests.ECommerce.Infrastructure;
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
            .WithCommand("--max_connections=500")
            .Build();

    private IDbConnection? _dbConnection;

    public CreateOrderTestFixture()
    {
        MySqlDbContainer.StartAsync().Wait();
        _dbConnection = new MySqlConnection(MySqlDbContainer.GetConnectionString());
        _dbConnection!.Execute(
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
          
          create table if not exists inventory (
              product_name varchar(255) not null primary key,
              quantity int not null,
              version_id int not null default 0
          );
          
          create index idx_inventory_product_name on inventory (product_name, version_id);
          """);
    }
    
    public IDbConnection CreateConnection()
    {
        return new MySqlConnection(MySqlDbContainer.GetConnectionString() + ";IgnoreCommandTransaction=true;");
    } 
    
    public async Task Insert(Customer customer)
    {
        var sql = """
                      insert into customers (id, name, is_premium)
                      values (@Id, @Name, @IsPremium);
                  """;
        await _dbConnection!.ExecuteAsync(sql, new
        {
            customer.Id,
            customer.Name,
            customer.IsPremium
        });
    }
    
    public async Task Insert(IEnumerable<ProductInventory> inventory)
    {
        var sql = """
                      insert into inventory (product_name, quantity)
                      values (@ProductName, @Quantity);
                  """;
        await _dbConnection!.ExecuteAsync(sql, inventory);
    }

    public UseCase.CreateOrder GetUseCase()
    {
        _dbConnection?.Close();
        _dbConnection = CreateConnection();
        _dbConnection.Open();
        return GetUseCase(_dbConnection);
    }

    public UseCase.CreateOrder GetUseCase(IDbConnection connection)
    {
        var customerRepository = new CustomerRepository(connection);
        var orderRepository = new OrderRepository(connection);
        var inventoryRepository = new InventoryRepository(connection);
        var unitOfWork = new UnitOfWork(connection);
        return new UseCase.CreateOrder(customerRepository, orderRepository, inventoryRepository, unitOfWork);
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
        return await _dbConnection!.QueryAsync<OrderModel>(sql);
    }
    
    public async Task<IEnumerable<ProductInventory>> GetInsertedInventory()
    {
        var sql = """
                      select 
                          product_name as ProductName,
                          quantity as Quantity,
                          version_id as VersionId
                      from inventory;
                  """;
        return await _dbConnection!.QueryAsync<ProductInventory>(sql);
    }
    
    public void CloseConnection()
    {
        _dbConnection?.Close();
        _dbConnection = null;
    }
    
    public void CleanUpDatabase()
    {
        _dbConnection!.Execute("""
                                  delete from orders;
                                  delete from customers;
                                  delete from inventory;
                              """);
    }
    
    public void Dispose()
    {
        _dbConnection?.Dispose();
        MySqlDbContainer.DisposeAsync()
            .GetAwaiter().GetResult();
    }
}