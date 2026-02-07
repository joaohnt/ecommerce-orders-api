using Ecommerce.Application.DTOs;
using Ecommerce.Application.Repositories;
using Ecommerce.Domain.Entities;
using Ecommerce.Worker.BackgroundJobs;
using Hangfire;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Ecommerce.Infrastructure.Consumer;

public class OrderCreatedConsumer : IConsumer<OrderCreatedPayload>
{
    private readonly ILogger<OrderCreatedConsumer> _logger;
    private readonly IOrderRepository _orderRepository;
    private readonly IBackgroundJobClient _backgroundJobClient;
    public OrderCreatedConsumer(ILogger<OrderCreatedConsumer> logger, IOrderRepository  orderRepository, IBackgroundJobClient backgroundJobClient)
    {
        _logger = logger;
        _orderRepository = orderRepository;
        _backgroundJobClient = backgroundJobClient;
    }
    
    public async Task Consume(ConsumeContext<OrderCreatedPayload> context)
    {
        _logger.LogInformation($"Order {context.Message.Id} received");
        
         _backgroundJobClient.Enqueue<ProcessOrderJob>(job => job.Handle(context.Message.Id));
    }
}