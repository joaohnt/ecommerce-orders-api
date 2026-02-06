using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.DTOs;

public class OrderDTO
{
    public List<OrderItemDTO> Items { get; set; } = new();
}