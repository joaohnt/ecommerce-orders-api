using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enums;

namespace Ecommerce.Tests.Entities;

public class OrderTests
{
     

    [Fact]
    public void AddOrderItem_WithInvalidPrice_ThenShouldThrowException()
    {
        var order = new Order();
        
        var act = () => order.AddOrderItem(2, "Mouse", 0);
        
        Assert.Throws<ArgumentOutOfRangeException>(act);
    }
    
    [Fact]
    public void AddOrderItem_WithInvalidQuantity_ThenShouldThrowException()
    {
        var order = new Order();
        
        var act = () => order.AddOrderItem(0, "Monitor", 10);
        
        Assert.Throws<ArgumentOutOfRangeException>(act);
    }
    
    [Fact]
    public void AddOrderItem_WithInvalidName_ThenShouldThrowException()
    {
        var order = new Order();
        
        var act = () => order.AddOrderItem(2, "", 10);
        
        Assert.Throws<ArgumentNullException>(act);
    }
    
    [Fact]
    public void AddOrderItem_WithValidParameters_ThenShouldAddItem()
    {
        var order = new Order();
        
        order.AddOrderItem(2, "Teclado", 522.12m);
        
        Assert.Single(order.OrderItems);
    }

    
}