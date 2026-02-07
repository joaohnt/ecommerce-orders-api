using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Repositories;

public interface IOrderRepository
{
    Task CreateOrder(Order order);
    Task<List<Order>> GetOrders();
    Task<Order> GetOrderById(int orderId);
    Task SaveAsync(Order order);
    Task<List<Order>> GetOrdersPaged(int page, int size);
    Task<int> GetOrdersCount();
}