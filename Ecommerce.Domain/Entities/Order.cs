using Ecommerce.Domain.Enums;

namespace Ecommerce.Domain.Entities;

public class Order
{
    public int Id { get; private set; }
    public Status Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    
    private readonly List<OrderItem> _orderItems = new(); 
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

    public Order()
    {
        Status = Status.Received;
        CreatedAt = DateTime.UtcNow;
    }
}