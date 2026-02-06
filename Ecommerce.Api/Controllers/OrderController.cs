using Ecommerce.Application.DTOs;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Service;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers;

[ApiController]
public class OrderController : ControllerBase
{
    private readonly ILogger<OrderController> _logger;
    private readonly IOrderService _orderService;
    public OrderController(ILogger<OrderController> logger, IOrderService orderService)
    {
        _logger = logger;
        _orderService = orderService;
    }

    [HttpPost]
    [Route("/order")]
    public async Task<IActionResult> CreateOrder([FromBody]OrderDTO order)
    {
        var newOrder = await _orderService.CreateOrder(order);
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
        return Ok(updatedOrder);
    }
    
    [HttpDelete]
    [Route("/order/{id}")]
    public async Task<IActionResult> CancelOrder([FromRoute]int id)
    {
        await _orderService.CancelOrder(id);
        return NoContent();
    }
}