using System.Text.Json.Serialization;

namespace Ecommerce.Domain.Entities;

public class OrderItem
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public int Quantity { get; private set; }
    
    public int OrderId { get; private set; }
    [JsonIgnore]
    public Order Order { get; private set; }
    
    internal OrderItem(string name, decimal price, int quantity)
    {
        Name = name;
        Price = price;
        Quantity = quantity;
    }
    
    internal void Update(string name, decimal price, int quantity)
    {
        Name = name;
        Price = price;
        Quantity = quantity;
    }
}