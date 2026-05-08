using Microsoft.AspNetCore.Mvc;
using Northwind.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Northwind.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly NorthwindContext _context;

    public CustomersController(NorthwindContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var customers = await _context.Customers
            .AsNoTracking()
            .Select(c => new { c.CustomerId, c.CompanyName, c.ContactName, c.City, c.Country })
            .OrderBy(c => c.CompanyName)
            .ToListAsync();
        return Ok(customers);
    }
}