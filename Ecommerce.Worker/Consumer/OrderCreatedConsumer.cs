using Ecommerce.Application.DTOs;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Ecommerce.Infrastructure.Consumer;

public class OrderCreatedConsumer : IConsumer<OrderDTO>
{
    private readonly ILogger<OrderCreatedConsumer> _logger;
    public OrderCreatedConsumer(ILogger<OrderCreatedConsumer> logger)
    {
        _logger = logger;
    }
    
    public Task Consume(ConsumeContext<OrderDTO> context)
    {
        var msg = context.Message;
        _logger.LogInformation("Order received");
        _logger.LogInformation("Order status changed to processed");
        return Task.CompletedTask;  
    }
}