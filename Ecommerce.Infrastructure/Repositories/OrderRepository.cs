using Ecommerce.Application.Repositories;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Database.Context;

namespace Ecommerce.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly EcommerceDbContext _context;
    public OrderRepository(EcommerceDbContext context)
    {
        _context = context;
    }
    
    public Task CreateOrder(Order order)
    {
        _context.Orders.AddAsync(order);
        return _context.SaveChangesAsync();
    }
}