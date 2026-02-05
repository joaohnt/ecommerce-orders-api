using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Service;

namespace Ecommerce.Application.Services;

public class OrderService : IOrderService
{
    public Task<Order> CreateOrder (Order order)
    {
        throw new NotImplementedException();
    }

    public Task<List<Order>> GetOrders()
    {
        throw new NotImplementedException();
    }

    public Task<Order> GetOrderById(int orderId)
    {
        throw new NotImplementedException();
    }

    public Task UpdateOrder(Order order)
    {
        throw new NotImplementedException();
    }
}