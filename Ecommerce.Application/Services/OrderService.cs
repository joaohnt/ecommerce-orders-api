using System.Reflection.Metadata.Ecma335;
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

    public async Task<List<Order>> GetOrders()
    {
        var orders =  await _orderRepository.GetOrders();
        return orders;
    }

    public async Task<Order> GetOrderById(int orderId)
    {
        var order = await _orderRepository.GetOrderById(orderId);
        return order;
    }

    public async Task<OrderDTO> UpdateOrder(int id, OrderDTO orderDto)
    {
        var order =  await _orderRepository.GetOrderById(id);
        
        order.ClearItemsToUpdate();
        foreach (var item in orderDto.Items )
            order.AddOrderItem(item.Quantity, item.Name, item.Price);
        
        await _orderRepository.SaveAsync(order);
        
        return orderDto;
    }

    public async Task CancelOrder(int orderId)
    {
        var order = await _orderRepository.GetOrderById(orderId);
        order.CancelOrder();
        
        await  _orderRepository.SaveAsync(order);
    }
}