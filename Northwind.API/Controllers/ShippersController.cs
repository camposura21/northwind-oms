using Microsoft.AspNetCore.Mvc;
using Northwind.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Northwind.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShippersController : ControllerBase
{
    private readonly NorthwindContext _context;

    public ShippersController(NorthwindContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var shippers = await _context.Shippers
            .AsNoTracking()
            .Select(s => new { s.ShipperId, s.CompanyName })
            .OrderBy(s => s.CompanyName)
            .ToListAsync();
        return Ok(shippers);
    }
}