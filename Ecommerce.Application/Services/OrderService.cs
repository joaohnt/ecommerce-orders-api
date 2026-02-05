using Ecommerce.Application.DTOs;
using Ecommerce.Application.Repositories;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Service;

namespace Ecommerce.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    
    public async Task<OrderDTO> CreateOrder (OrderDTO orderDto)
    {
        var order = new Order();
        
        foreach (var item in orderDto.Items)
            order.AddOrderItem(item.Quantity, item.Name, item.Price);
        
        await _orderRepository.CreateOrder(order);
        return orderDto;
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