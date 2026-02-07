using Ecommerce.Application.Repositories;
using Hangfire;

namespace Ecommerce.Worker.BackgroundJobs;

public class ProcessOrderJob
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<ProcessOrderJob> _logger;
    public ProcessOrderJob(IOrderRepository orderRepository, ILogger<ProcessOrderJob> logger)
    {
        _orderRepository = orderRepository;
        _logger = logger;
    }

    [Queue("default")]
    public async Task Handle(int OrderId)
    {
        var order = await _orderRepository.GetOrderById(OrderId);
        _logger.LogInformation($"Order {OrderId} consumed in worker");
        order.UpdateOrderStatusToProcessed();
        await _orderRepository.SaveAsync(order);
        _logger.LogInformation($"Order {OrderId} processed");
    }
}