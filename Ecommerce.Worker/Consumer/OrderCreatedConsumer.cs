using Ecommerce.Application.DTOs;
using Ecommerce.Application.Repositories;
using Ecommerce.Domain.Entities;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Ecommerce.Infrastructure.Consumer;

public class OrderCreatedConsumer : IConsumer<OrderCreatedPayload>
{
    private readonly ILogger<OrderCreatedConsumer> _logger;
    private readonly IOrderRepository _orderRepository;
    public OrderCreatedConsumer(ILogger<OrderCreatedConsumer> logger, IOrderRepository  orderRepository)
    {
        _logger = logger;
        _orderRepository = orderRepository;
    }
    
    public async Task Consume(ConsumeContext<OrderCreatedPayload> context)
    {
        _logger.LogInformation("Order received");
        
        var order = await _orderRepository.GetOrderById(context.Message.Id);
        order.UpdateOrderStatusToProcessed();
        await _orderRepository.SaveAsync(order);
        _logger.LogInformation("Order status changed to processed");
    }
}