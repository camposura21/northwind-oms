using Microsoft.AspNetCore.Mvc;
using Northwind.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Northwind.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly NorthwindContext _context;

    public ReportsController(NorthwindContext context)
    {
        _context = context;
    }

    [HttpGet("orders-by-month")]
    public async Task<IActionResult> GetOrdersByMonth([FromQuery] int? year)
    {
        var orders = await _context.Orders
            .AsNoTracking()
            .Where(o => o.OrderDate.HasValue && (!year.HasValue || o.OrderDate.Value.Year == year.Value))
            .Select(o => new { o.OrderDate, o.Freight })
            .ToListAsync();

        var data = orders
            .GroupBy(o => new { o.OrderDate!.Value.Year, o.OrderDate.Value.Month })
            .Select(g => new {
                year = g.Key.Year,
                month = g.Key.Month,
                count = g.Count(),
                total = g.Sum(o => o.Freight ?? 0)
            })
            .OrderBy(x => x.year).ThenBy(x => x.month)
            .ToList();

        return Ok(data);
    }

    [HttpGet("shipments-by-region")]
    public async Task<IActionResult> GetShipmentsByRegion()
    {
        var orders = await _context.Orders
            .AsNoTracking()
            .Where(o => o.ShipCountry != null)
            .Select(o => new { o.ShipCountry })
            .ToListAsync();

        var data = orders
            .GroupBy(o => o.ShipCountry)
            .Select(g => new {
                region = g.Key,
                count = g.Count()
            })
            .OrderByDescending(x => x.count)
            .Take(8)
            .ToList();

        return Ok(data);
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary()
    {
        var orders = await _context.Orders
            .AsNoTracking()
            .Select(o => new { o.ShippedDate, o.Freight })
            .ToListAsync();

        var total = orders.Count;
        var shipped = orders.Count(o => o.ShippedDate != null);
        var pending = total - shipped;
        var freight = orders.Sum(o => o.Freight ?? 0);

        return Ok(new { total, shipped, pending, freight });
    }
    [HttpGet("shipment-coordinates")]
public async Task<IActionResult> GetShipmentCoordinates()
{
    var orders = await _context.Orders
        .AsNoTracking()
        .Where(o => o.ShipCountry != null)
        .Select(o => new {
            o.OrderId,
            o.ShipCity,
            o.ShipCountry,
            o.CustomerId,
            o.Freight
        })
        .Take(200)
        .ToListAsync();
    return Ok(orders);
}
}
