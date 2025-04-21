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
    }

    public void Dispose()
    {
        _fixture.CleanUpDatabase();
        _fixture.CloseConnection();
    }
}