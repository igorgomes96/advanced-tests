namespace AdvancedTests.ECommerce.IntegrationTests.Models;

public class OrderModel
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int Status { get; set; }
    public string State { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Street { get; set; } = null!;
    public string ZipCode { get; set; } = null!;
    public int Number { get; set; }
    public decimal Amount { get; set; }
}