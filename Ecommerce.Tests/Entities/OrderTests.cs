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
    public void ClearItemsToUpdate_WithValidStatus_ThenShouldClearItems()
    {
        var order = new Order();
        order.AddOrderItem(1, "Mouse", 10);
        
        order.ClearItemsToUpdate();
        
        Assert.NotNull(order.UpdatedAt); 
    }
    
    [Fact]
    public void ClearItemsToUpdate_WithStatusProcessed_ShouldThrow()
    {
        var order = new Order();
        typeof(Order).GetProperty("Status")!.SetValue(order, Status.Processed);

        var act = () => order.ClearItemsToUpdate();

        Assert.Throws<InvalidOperationException>(act);
    }

    [Fact]
    public void ClearItemsToUpdate_WithStatusCanceled_ShouldThrow()
    {
        var order = new Order();
        typeof(Order).GetProperty("Status")!.SetValue(order, Status.Canceled);

        var act = () => order.ClearItemsToUpdate();

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