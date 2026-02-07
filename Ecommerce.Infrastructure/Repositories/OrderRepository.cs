using Ecommerce.Application.Repositories;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enums;
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

    public Task SaveAsync(Order order)
    {
        return _context.SaveChangesAsync();
    }

    public Task<List<Order>> GetOrdersPaged(int page, int size, Status? status=null)
    {
        var query = _context.Orders
            .Include(o => o.OrderItems)
            .AsNoTracking()
            .AsQueryable();
            if(status.HasValue)
                query = query.Where(o => o.Status == status.Value);
                    
        return query
            .OrderByDescending(o => o.Id) 
            .Skip(page * size)
            .Take(size)
            .ToListAsync();
    }

    public Task<int> GetOrdersCount(Status? status=null)
    {
        var query = _context.Orders.AsQueryable();

        if (status.HasValue)
            query = query.Where(o => o.Status == status.Value);

        return query.CountAsync();
    }
}