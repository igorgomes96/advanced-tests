using AdvancedTests.ECommerce.Domain;
using AdvancedTests.ECommerce.Domain.Entities;
using AdvancedTests.ECommerce.Domain.Repositories;

namespace AdvancedTests.ECommerce.Application.UseCases.CreateOrder;

public class CreateOrder(
    ICustomerRepository customerRepository,
    IOrderRepository orderRepository,
    IInventoryRepository inventoryRepository,
    IUnitOfWork unitOfWork) : ICreateOrder
{
    public async Task<CreateOrderOutput> ExecuteAsync(CreateOrderInput input)
    {
        await unitOfWork.BeginTransactionAsync();
        var customer = await customerRepository.GetByIdAsync(input.CustomerId);
        if (customer == null)
        {
            throw new InvalidOperationException("Customer not found");
        }

        var inventoryItems = await CheckInventoryAsync(input.Items);

        var order = new Order(customer, input.Address.ToAddress(),
            input.Items.Select(i => i.ToOrderItem()).ToList());
        var orderId = await orderRepository.AddAsync(order);
        await inventoryRepository.UpdateAsync(inventoryItems);
        await unitOfWork.CommitAsync();
        return new CreateOrderOutput(orderId);
    }
    
    private async Task<IEnumerable<ProductInventory>> CheckInventoryAsync(IEnumerable<CreateOrderItemInput> items)
    {
        var productNames = items.Select(i => i.Name).Distinct().ToList();
        var inventoryItems = (await inventoryRepository
            .GetByProductNamesAsync(productNames))
            .ToDictionary(x => x.ProductName);

        foreach (var item in items)
        {
            if (!inventoryItems.TryGetValue(item.Name, out var inventoryItem))
            {
                throw new InvalidOperationException($"Product '{item.Name}' not found in inventory.");
            }

            if (inventoryItem.Quantity < item.Quantity)
            {
                throw new InvalidOperationException($"Insufficient stock for product '{item.Name}'.");
            }
            
            inventoryItem.Update(inventoryItem.Quantity - item.Quantity);
        }
        return inventoryItems.Values;
    }
}