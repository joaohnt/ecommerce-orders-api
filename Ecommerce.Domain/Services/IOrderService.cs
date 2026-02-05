using Ecommerce.Domain.Entities;

namespace Ecommerce.Domain.Service;

public interface IOrderService
{
    Task<Order> CreateOrder(Order order);
    Task<List<Order>> GetOrders();
    Task<Order> GetOrderById(int orderId);
    Task UpdateOrder(Order order);
}