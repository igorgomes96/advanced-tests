using AdvancedTests.ECommerce.Domain.Entities;
using FluentAssertions;
using Moq;
using static AdvancedTests.ECommerce.UnitTests.DataBuilders.CustomerDataBuilder;
using static AdvancedTests.ECommerce.UnitTests.DataBuilders.CreateOrderInputDataBuilder;

namespace AdvancedTests.ECommerce.UnitTests.Application.UseCases.CreateOrder;

public class CreateOrderTest(CreateOrderTestFixture fixture) : IClassFixture<CreateOrderTestFixture>
{
    [Fact]
    public async Task ThrowsInvalidOperationExceptionWhenCustomerIdIsNotFound()
    {
        var customerId = 999;
        fixture.CustomerRepository
            .Setup(repo => repo.GetByIdAsync(customerId))
            .ReturnsAsync((Customer?)null);

        var anInput = AnOrderInput()
            .FromCustomerId(customerId)
            .Build();

        var action = async () => await fixture.UseCase.ExecuteAsync(anInput);

        await action.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Customer not found");
    }

    [Fact]
    public async Task CreateOrderAndReturnInsertedIdWhenInputIsValid()
    {
        var aCustomer = ARegularCustomer().Build();
        fixture.CustomerRepository
            .Setup(repo => repo.GetByIdAsync(aCustomer.Id))
            .ReturnsAsync(aCustomer);

        const int outputId = 1;
        fixture.OrderRepository
            .Setup(repo => repo.AddAsync(It.IsAny<Order>()))
            .ReturnsAsync(outputId);

        var anInput = AnOrderInput()
            .FromCustomerId(aCustomer.Id)
            .Build();

        var inventory = anInput.Items
            .GroupBy(item => item.Name)
            .Select(group => new ProductInventory(group.Key, group.Sum(item => item.Quantity)));
        
        fixture.InventoryRepository.Setup(x =>
                x.GetByProductNamesAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(inventory);

        var output = await fixture.UseCase.ExecuteAsync(anInput);

        output.Id.Should().Be(outputId);
        fixture.UnitOfWork.Verify(uof => uof.BeginTransactionAsync(), Times.Once);
        fixture.UnitOfWork.Verify(uof => uof.CommitAsync(), Times.Once);
    }
}