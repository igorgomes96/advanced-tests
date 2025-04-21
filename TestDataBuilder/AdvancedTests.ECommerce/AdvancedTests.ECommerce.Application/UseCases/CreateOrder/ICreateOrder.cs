namespace AdvancedTests.ECommerce.Application.UseCases.CreateOrder;

public interface ICreateOrder
{
    Task<CreateOrderOutput> ExecuteAsync(CreateOrderInput input);
}