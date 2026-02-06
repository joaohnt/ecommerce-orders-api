using Ecommerce.Application.Repositories;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;

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

    public Task<List<Order>> GetOrders()
    {
        return _context.Orders.Include(i => i.OrderItems).AsNoTracking().ToListAsync();
    }

    public Task<Order> GetOrderById(int orderId)
    {
        return _context.Orders.Include(i => i.OrderItems).Where(o => o.Id == orderId).FirstOrDefaultAsync();
    }

    public Task UpdateOrder(Order order)
    {
        return _context.SaveChangesAsync();
    }
}