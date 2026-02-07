using Ecommerce.Application.DTOs;
using Ecommerce.Application.Pagination;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Domain.Service;

public interface IOrderService
{
    Task<Order> CreateOrder(OrderDTO order);
    Task<List<Order>> GetOrders();
    Task<Order> GetOrderById(int orderId);
    Task<OrderDTO> UpdateOrder(int orderId, OrderDTO order);
    Task CancelOrder(int orderId);
    Task<PagedResult<Order>> GetOrdersPaged(int page, int size);
}