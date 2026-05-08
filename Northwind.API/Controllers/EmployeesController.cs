using Microsoft.AspNetCore.Mvc;
using Northwind.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Northwind.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly NorthwindContext _context;

    public EmployeesController(NorthwindContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var employees = await _context.Employees
            .AsNoTracking()
            .Select(e => new { e.EmployeeId, FullName = e.FirstName + " " + e.LastName })
            .OrderBy(e => e.FullName)
            .ToListAsync();
        return Ok(employees);
    }
}