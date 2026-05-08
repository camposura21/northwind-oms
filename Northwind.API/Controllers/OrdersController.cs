using Microsoft.AspNetCore.Mvc;
using Northwind.Application.Interfaces;
using Northwind.Domain.Entities;
using Northwind.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Northwind.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderRepository _orderRepository;
    private readonly NorthwindContext _context;

    public OrdersController(IOrderRepository orderRepository, NorthwindContext context)
    {
        _orderRepository = orderRepository;
        _context = context;
    }

    [HttpGet]
    public async Task<IEnumerable<Order>> GetAllAsync()
{
    return await _context.Orders
        .AsNoTracking()
        .Include(o => o.Customer)
        .Include(o => o.OrderDetails)
            .ThenInclude(od => od.Product)
        .ToListAsync();
}
    public async Task<IActionResult> GetById(int id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order == null) return NotFound();
        return Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] OrderDto dto)
    {
        var order = new Order
        {
            CustomerId = dto.CustomerId,
            EmployeeId = dto.EmployeeId,
            OrderDate = dto.OrderDate,
            RequiredDate = dto.RequiredDate,
            ShippedDate = dto.ShippedDate,
            ShipVia = dto.ShipVia,
            Freight = dto.Freight,
            ShipName = dto.ShipName,
            ShipAddress = dto.ShipAddress,
            ShipCity = dto.ShipCity,
            ShipRegion = dto.ShipRegion,
            ShipPostalCode = dto.ShipPostalCode,
            ShipCountry = dto.ShipCountry
        };

        if (dto.OrderDetails != null)
        {
            foreach (var d in dto.OrderDetails)
            {
                order.OrderDetails.Add(new OrderDetail
                {
                    ProductId = d.ProductId,
                    UnitPrice = d.UnitPrice,
                    Quantity = d.Quantity,
                    Discount = d.Discount
                });
            }
        }

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = order.OrderId }, order);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] OrderDto dto)
    {
        var order = await _context.Orders
            .Include(o => o.OrderDetails)
            .FirstOrDefaultAsync(o => o.OrderId == id);

        if (order == null) return NotFound();

        order.CustomerId = dto.CustomerId;
        order.EmployeeId = dto.EmployeeId;
        order.OrderDate = dto.OrderDate;
        order.RequiredDate = dto.RequiredDate;
        order.ShippedDate = dto.ShippedDate;
        order.ShipVia = dto.ShipVia;
        order.Freight = dto.Freight;
        order.ShipName = dto.ShipName;
        order.ShipAddress = dto.ShipAddress;
        order.ShipCity = dto.ShipCity;
        order.ShipRegion = dto.ShipRegion;
        order.ShipPostalCode = dto.ShipPostalCode;
        order.ShipCountry = dto.ShipCountry;

        _context.OrderDetails.RemoveRange(order.OrderDetails);

        if (dto.OrderDetails != null)
        {
            foreach (var d in dto.OrderDetails)
            {
                order.OrderDetails.Add(new OrderDetail
                {
                    OrderId = id,
                    ProductId = d.ProductId,
                    UnitPrice = d.UnitPrice,
                    Quantity = d.Quantity,
                    Discount = d.Discount
                });
            }
        }

        await _context.SaveChangesAsync();
        return Ok(order);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _orderRepository.DeleteAsync(id);
        return NoContent();
    }
}

public class OrderDto
{
    public string? CustomerId { get; set; }
    public int? EmployeeId { get; set; }
    public DateTime? OrderDate { get; set; }
    public DateTime? RequiredDate { get; set; }
    public DateTime? ShippedDate { get; set; }
    public int? ShipVia { get; set; }
    public decimal? Freight { get; set; }
    public string? ShipName { get; set; }
    public string? ShipAddress { get; set; }
    public string? ShipCity { get; set; }
    public string? ShipRegion { get; set; }
    public string? ShipPostalCode { get; set; }
    public string? ShipCountry { get; set; }
    public List<OrderDetailDto>? OrderDetails { get; set; }
}

public class OrderDetailDto
{
    public int ProductId { get; set; }
    public decimal UnitPrice { get; set; }
    public short Quantity { get; set; }
    public float Discount { get; set; }
}