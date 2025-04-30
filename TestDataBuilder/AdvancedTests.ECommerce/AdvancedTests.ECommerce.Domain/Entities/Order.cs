using Ardalis.GuardClauses;

namespace AdvancedTests.ECommerce.Domain.Entities;

public class Order
{
    public int Id { get; }
    public Customer Customer { get; }
    public Address Address { get; }
    public decimal Amount { get; private set; }
    public OrderStatus Status { get; private set; }

    private readonly List<OrderItem> _items = [];
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    public Order(Customer customer, Address address, IEnumerable<OrderItem> items)
    {
        Customer = Guard.Against.Null(customer);
        Address = Guard.Against.Null(address);
        Guard.Against.NullOrEmpty(items);
        _items.AddRange(items);
        CalculateAmount();
        Status = OrderStatus.Created;
    }

    private void CalculateAmount()
    {
        decimal discount = 0;
        var total = _items.Sum(item => item.Total);
        if (total > 1000 && Customer.IsPremium)
        {
            discount = 0.1m;
        }
        Amount = total * (1 - discount);
    }

    public void AddItem(OrderItem item)
    {
        Guard.Against.Null(item);
        if (Status != OrderStatus.Created && Status != OrderStatus.Confirmed)
            throw new InvalidOperationException("Item não pode ser adicionado ao pedido.");
        _items.Add(item);
        CalculateAmount();
    }

    public void Confirm()
    {
        if (Status != OrderStatus.Created)
            throw new InvalidOperationException("Somente itens no status 'Criado' podem ser confirmados.");
        Status = OrderStatus.Confirmed;
    }

    public void Pay()
    {
        if (Status != OrderStatus.Confirmed)
            throw new InvalidOperationException("Somente itens no status 'Confirmado' podem ser pagos.");
        Status = OrderStatus.Paid;
    }

    public void Ship()
    {
        if (Status != OrderStatus.Paid)
            throw new InvalidOperationException("Somente itens no status 'Pago' podem ser enviados.");
        Status = OrderStatus.Shipped;
    }

    public void Deliver()
    {
        if (Status != OrderStatus.Shipped)
            throw new InvalidOperationException("Somente itens no status 'Enviado' podem ser entregues.");
        Status = OrderStatus.Delivered;
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Delivered)
            throw new InvalidOperationException("Itens entregues não podem ser cancelados.");
        Status = OrderStatus.Cancelled;
    }
}