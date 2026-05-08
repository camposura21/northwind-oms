using Microsoft.EntityFrameworkCore;
using Northwind.Application.Interfaces;
using Northwind.Domain.Entities;
using Northwind.Infrastructure.Data;

namespace Northwind.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly NorthwindContext _context;

    public OrderRepository(NorthwindContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        return await _context.Orders
            .AsNoTracking()
            .Include(o => o.Customer)
            .ToListAsync();
    }

    public async Task<Order?> GetByIdAsync(int id)
    {
        return await _context.Orders
            .AsNoTracking()
            .Include(o => o.Customer)
            .Include(o => o.Employee)
            .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
            .FirstOrDefaultAsync(o => o.OrderId == id);
    }

    public async Task<Order> CreateAsync(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task<Order> UpdateAsync(Order order)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
        return order;
    }
public async Task DeleteAsync(int id)
{
    var order = await _context.Orders
        .Include(o => o.OrderDetails)
        .FirstOrDefaultAsync(o => o.OrderId == id);
    if (order != null)
    {
        _context.OrderDetails.RemoveRange(order.OrderDetails);
        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
    }
}
}