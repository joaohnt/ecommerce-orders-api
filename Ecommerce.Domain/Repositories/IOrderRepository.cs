using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Repositories;

public interface IOrderRepository
{
    Task CreateOrder(Order order);
}