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
        var orders =  _orderRepository.GetOrders();
        return orders;
    }

    public Task<Order> GetOrderById(int orderId)
    {
        var order = _orderRepository.GetOrderById(orderId);
        return order;
    }

    public Task UpdateOrder(Order order)
    {
        throw new NotImplementedException();
    }
}