using System.Reflection.Metadata.Ecma335;
using Ecommerce.Application.DTOs;
using Ecommerce.Application.Pagination;
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
    
    public async Task<Order> CreateOrder (OrderDTO orderDto)
    {
        var order = new Order();
        
        foreach (var item in orderDto.Items)
            order.AddOrderItem(item.Quantity, item.Name, item.Price);
        order.EnsureHasItems();
        
        await _orderRepository.CreateOrder(order);
        return order;
    }

    public async Task<List<Order>> GetOrders()
    {
        var orders =  await _orderRepository.GetOrders();
        return orders;
    }

    public async Task<Order> GetOrderById(int orderId)
    {
        var order = await _orderRepository.GetOrderById(orderId);
        if(order == null)
            throw new KeyNotFoundException($"O pedido de id: {orderId} não existe. ");
        return order;
    }

    public async Task<OrderDTO> UpdateOrder(int id, OrderDTO orderDto)
    {
        var order =  await _orderRepository.GetOrderById(id);
        if (order == null)
            throw new KeyNotFoundException($"O pedido de id: {id} não existe. ");
        
        order.ClearItemsToUpdate();
        foreach (var item in orderDto.Items )
            order.AddOrderItem(item.Quantity, item.Name, item.Price);
        
        await _orderRepository.SaveAsync(order);
        
        return orderDto;
    }

    public async Task CancelOrder(int orderId)
    {
        var order = await _orderRepository.GetOrderById(orderId);
        if (order == null)
            throw new KeyNotFoundException($"O pedido de id: {orderId} não existe. ");
        order.CancelOrder();
        
        await  _orderRepository.SaveAsync(order);
    }

    public async Task<PagedResult<Order>> GetOrdersPaged(int page, int size)
    {
        var items = await _orderRepository.GetOrdersPaged(page, size);
        var total = await _orderRepository.GetOrdersCount();

        return new PagedResult<Order>
        {
            Items = items,
            PageNumber = page,
            PageSize = size,
            TotalRecords = total
        };

    }
}