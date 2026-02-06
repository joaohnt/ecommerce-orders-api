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

    public void EnsureHasItems()
    {
        if (_orderItems.Count == 0)
            throw new ArgumentException("A lista de itens não pode ser vazia");
    }
    private void CheckItem(string name, decimal price, int quantity)
    {
        if(quantity < 1)
            throw new ArgumentOutOfRangeException(nameof(quantity), "A quantidade não pode ser inferior a 1");
        if(price <= 0)
            throw new ArgumentOutOfRangeException(nameof(price), "O preço precisa ser superior a 0");
        if(string.IsNullOrEmpty(name)) 
            throw new ArgumentNullException(nameof(name), "O nome não pode ser vazio");
    }
    
    public void ClearItemsToUpdate()
    {
        if (Status == Status.Processed)
            throw new  InvalidOperationException("Não é possível alterar um pedido já processado");
        if (Status == Status.Canceled)
            throw new InvalidOperationException("Não é possivel alterar um pedido cancelado");
        _orderItems.Clear();
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
            throw new InvalidOperationException("O pedido já está cancelado.");
        
        Status =  Status.Canceled;
        UpdatedAt = DateTime.UtcNow;
    }
}