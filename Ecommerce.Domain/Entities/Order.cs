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
    
    private void CheckItem(string name, decimal price, int quantity)
    {
        if(quantity < 1)
            throw new ArgumentOutOfRangeException(nameof(quantity), "qtd maior tem q ser >0");
        if(price <= 0)
            throw new ArgumentOutOfRangeException(nameof(price), "preco tem q ser >0");
        if(string.IsNullOrEmpty(name)) 
            throw new ArgumentNullException(nameof(name));
    }

    public void UpdateOrder(int quantity, string name, decimal price, int itemId)
    {
        var item = _orderItems.FirstOrDefault(i => i.Id == itemId);
        if(item == null)
            throw new ArgumentException("item nao pertence ao pedido");
        if (Status == Status.Processed)
            throw new  InvalidOperationException("Não é possível alterar um pedido já processado (400 bad request)");
        if (Status == Status.Canceled)
            throw new InvalidOperationException(" n pode alterar pedido cancelado");
        
        CheckItem(name, price, quantity);
        
        item.Update(name, price, quantity);
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddOrderItem(int quantity, string name, decimal price)
    {
        CheckItem(name, price, quantity);
        _orderItems.Add(new OrderItem(name, price, quantity));
    }

    public void CancelOrder()
    {
        if(Status == Status.Canceled)
            throw  new InvalidOperationException("Pedido ja esta cancelado");
        
        Status =  Status.Canceled;
        UpdatedAt = DateTime.UtcNow;
    }
}