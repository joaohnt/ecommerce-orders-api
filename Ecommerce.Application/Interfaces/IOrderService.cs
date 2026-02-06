using Ecommerce.Application.DTOs;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Domain.Service;

public interface IOrderService
{
    Task<OrderDTO> CreateOrder(OrderDTO order);
    Task<List<Order>> GetOrders();
    Task<Order> GetOrderById(int orderId);
    Task<OrderDTO> UpdateOrder(int orderId, OrderDTO order);
}