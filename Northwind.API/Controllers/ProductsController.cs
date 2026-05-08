using Microsoft.AspNetCore.Mvc;
using Northwind.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Northwind.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly NorthwindContext _context;

    public ProductsController(NorthwindContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _context.Products
            .AsNoTracking()
            .Where(p => !p.Discontinued)
            .Select(p => new {
                p.ProductId,
                p.ProductName,
                p.UnitPrice,
                p.UnitsInStock,
                CategoryName = p.Category != null ? p.Category.CategoryName : ""
            })
            .OrderBy(p => p.ProductName)
            .ToListAsync();
        return Ok(products);
    }
}