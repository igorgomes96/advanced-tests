using AdvancedTests.ECommerce.Domain;
using AdvancedTests.ECommerce.Domain.Repositories;
using Moq;
using CreateOrderUseCase = AdvancedTests.ECommerce.Application.UseCases.CreateOrder;

namespace AdvancedTests.ECommerce.UnitTests.Application.UseCases.CreateOrder;

public class CreateOrderTestFixture
{
    public Mock<ICustomerRepository> CustomerRepository { get; }
    public Mock<IOrderRepository> OrderRepository { get; }
    public Mock<IUnitOfWork> UnitOfWork { get; }
    public CreateOrderUseCase.CreateOrder UseCase { get; }

    public CreateOrderTestFixture()
    {
        CustomerRepository = new Mock<ICustomerRepository>();
        OrderRepository = new Mock<IOrderRepository>();
        UnitOfWork = new Mock<IUnitOfWork>();
        UseCase = new CreateOrderUseCase.CreateOrder(
            CustomerRepository.Object,
            OrderRepository.Object,
            UnitOfWork.Object);
    }
}