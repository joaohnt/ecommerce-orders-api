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
    [Fact]
    public void UpdateOrder_WithValidItemId_ThenShouldUpdateItem()
    {
        var order = new Order();
        var itemId = 0;
        order.AddOrderItem(1, "Mouse", 10);
        
        order.UpdateOrder(2, "Mouse", 25, itemId); 
        
        var updated = order.OrderItems.First();
        Assert.Equal(2, updated.Quantity);
        Assert.Equal(25, updated.Price);
        Assert.NotNull(order.UpdatedAt); 
    }
    
    [Fact]
    public void UpdateOrder_WithInvalidItemId_ThenShouldThrowException()
    {
        var order = new Order();
        var itemId = 5;
        order.AddOrderItem(1, "Mouse", 10);
        
        var act = () => order.UpdateOrder(2, "Mouse", 25, itemId); 
        
        Assert.Throws<ArgumentException>(act);
    }
    
    [Theory]
    [InlineData(Status.Canceled)]
    [InlineData(Status.Processed)]
    public void UpdateOrder_WithInvalidStatus_ThenShouldThrowException(Status status)
    {
        var order = new Order();
        typeof(Order)
            .GetProperty("Status")!
            .SetValue(order, status);        
        var itemId = 0;
        order.AddOrderItem(1, "Mouse", 10);
        
        var act = () => order.UpdateOrder(2, "Mouse", 25, itemId); 
        
        Assert.Throws<InvalidOperationException>(act);
    }
    
    [Fact]
    public void CancelOrder_WithValidParams_ThenShouldCancelOrder()
    {
        var order = new Order();
        order.AddOrderItem(2, "Teclado", 522.12m);
        
        order.CancelOrder();
        
        Assert.Equal(Status.Canceled, order.Status);
    }
    
    [Fact]
    public void CancelOrder_WithInvalidStatus_ThenShouldThrowExceptionr()
    {
        var order = new Order();
        typeof(Order)
            .GetProperty("Status")!
            .SetValue(order, Status.Canceled);       
        order.AddOrderItem(2, "Teclado", 522.12m);
        
        var act = () => order.CancelOrder();
        
        
        Assert.Throws<InvalidOperationException>(act);
    }
    
}