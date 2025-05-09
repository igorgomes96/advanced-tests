using AdvancedTests.ECommerce.Domain.Entities;

namespace AdvancedTests.ECommerce.Application.UseCases.ListOrders;

public record ListOrdersOutput(
    int Id,
    string CustomerName,
    decimal Amount,
    OrderStatus Status);
    