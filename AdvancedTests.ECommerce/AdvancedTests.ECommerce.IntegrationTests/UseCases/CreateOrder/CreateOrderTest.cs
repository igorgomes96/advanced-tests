using AdvancedTests.ECommerce.Domain.Entities;
using FluentAssertions;
using UseCase = AdvancedTests.ECommerce.Application.UseCases.CreateOrder;
using static AdvancedTests.ECommerce.IntegrationTests.DataBuilders.CustomerDataBuilder;
using static AdvancedTests.ECommerce.IntegrationTests.DataBuilders.CreateOrderInputDataBuilder;

namespace AdvancedTests.ECommerce.IntegrationTests.UseCases.CreateOrder;

public class CreateOrderTest : IClassFixture<CreateOrderTestFixture>, IDisposable
{
    private readonly CreateOrderTestFixture _fixture;
    private readonly UseCase.CreateOrder _useCase;

    public CreateOrderTest(CreateOrderTestFixture fixture)
    {
        _fixture = fixture;
        _useCase = fixture.GetUseCase();
    }

    [Fact]
    public async Task ThrowsInvalidOperationExceptionWhenCustomerIdIsNotFound()
    {
        var anInput = AnOrderInput()
            .FromCustomerId(999)
            .Build();

        var action = async () => await _useCase.ExecuteAsync(anInput);

        await action.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Customer not found");
    }

    [Fact]
    public async Task CreateOrderAndReturnInsertedIdWhenInputIsValid()
    {
        var aCustomer = ARegularCustomer().Build();
        await _fixture.Insert(aCustomer);
        var anInput = AnOrderInput()
            .FromCustomerId(aCustomer.Id)
            .Build();
        var inventory = anInput.Items
            .GroupBy(item => item.Name)
            .Select(group => new ProductInventory(group.Key, group.Sum(item => item.Quantity)));
        await _fixture.Insert(inventory);

        var output = await _useCase.ExecuteAsync(anInput);

        output.Should().NotBe(default(int));
        var insertedOrders = await _fixture.GetInsertedOrders();
        insertedOrders.Should().HaveCount(1)
            .And.Subject.Single()
            .Should()
            .BeEquivalentTo(new
            {
                output.Id,
                CustomerId = aCustomer.Id,
                anInput.Address.State,
                anInput.Address.City,
                anInput.Address.Street,
                anInput.Address.ZipCode,
                anInput.Address.Number,
                Amount = anInput.Items.Sum(i => i.Price * i.Quantity)
            });
        var insertedInventory = await _fixture.GetInsertedInventory();
        insertedInventory.Should().HaveCount(anInput.Items.Count())
            .And.Subject
            .Should()
            .AllSatisfy(item => item.Quantity.Should().Be(0));
    }
    
    [Fact]
    public async Task CreateOrdersWhileInventoryIsAvailable()
    {
        const int expectedOrdersCount = 3;
        const int expectedRequestCount = 300;
        
        var aCustomer = ARegularCustomer().Build();
        await _fixture.Insert(aCustomer);
        var anInput = AnOrderInput()
            .FromCustomerId(aCustomer.Id)
            .Build();
        var inventory = anInput.Items
            .GroupBy(item => item.Name)
            .Select(group => new ProductInventory(group.Key, group.Sum(item => item.Quantity) * expectedOrdersCount));
        await _fixture.Insert(inventory);

        var rand = new Random();
        var tasks = Enumerable.Range(0, expectedRequestCount)
            .Select(async _ =>
            {
                using var connection = _fixture.CreateConnection();
                connection.Open();
                var useCase = _fixture.GetUseCase(connection);
                try
                {
                    await Task.Delay(rand.Next(0, 1000));
                    await useCase.ExecuteAsync(anInput);
                    return true;
                }
                catch
                {
                    return false;
                }
            });
        
        var results = await Task.WhenAll(tasks);

        results.Where(result => result).Should().HaveCount(expectedOrdersCount);
        var insertedOrders = await _fixture.GetInsertedOrders();
        insertedOrders.Should().HaveCount(expectedOrdersCount);
        var insertedInventory = await _fixture.GetInsertedInventory();
        insertedInventory.Should().HaveCount(anInput.Items.Count())
            .And.Subject
            .Should()
            .AllSatisfy(item => item.Quantity.Should().Be(0));
    }

    public void Dispose()
    {
        _fixture.CleanUpDatabase();
        _fixture.CloseConnection();
    }
}