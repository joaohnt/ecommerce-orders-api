using Ecommerce.Application.DTOs;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Service;
using Ecommerce.Infrastructure.Consumer;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers;

[ApiController]
public class OrderController : ControllerBase
{
    private readonly ILogger<OrderController> _logger;
    private readonly IPublishEndpoint _publisher;
    private readonly IOrderService _orderService;
    public OrderController(ILogger<OrderController> logger, IOrderService orderService,  IPublishEndpoint publisher)
    {
        _logger = logger;
        _orderService = orderService;
        _publisher = publisher;
    }

    [HttpPost]
    [Route("/order")]
    public async Task<IActionResult> CreateOrder([FromBody]OrderDTO order)
    {
        var newOrder = await _orderService.CreateOrder(order);
        
        await _publisher.Publish(new OrderCreatedPayload(newOrder.Id));
        _logger.LogInformation("Order created");
        
        return Created("",  newOrder);
    }
    
    [HttpGet]
    [Route("/orders")]
    public async Task<IActionResult> GetOrders()
    {
        var orders =  await _orderService.GetOrders();
        return Ok(orders);
    }
    
    [HttpGet]
    [Route("/order/{id}")]
    public async Task<IActionResult> GetOrders([FromRoute]int id)
    {
        var orders =  await _orderService.GetOrderById(id);
        return Ok(orders);
    }
    
    [HttpPut]
    [Route("/order/{id}")]
    public async Task<IActionResult> GetOrderById([FromRoute]int id, [FromBody]OrderDTO order)
    {
        var updatedOrder = await _orderService.UpdateOrder(id, order);
        _logger.LogInformation("Order updated");
        return Ok(updatedOrder);
    }
    
    [HttpDelete]
    [Route("/order/{id}")]
    public async Task<IActionResult> CancelOrder([FromRoute]int id)
    {
        await _orderService.CancelOrder(id);
        _logger.LogInformation("Order canceled");
        return NoContent();
    }
}