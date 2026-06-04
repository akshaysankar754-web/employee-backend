using EmployeePortalAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeePortalAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public DashboardController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetDashboard()
    {
        var totalEmployees =
            await _context.Employees.CountAsync();

        var totalDepartments =
            await _context.Employees
                .Select(x => x.Department)
                .Distinct()
                .CountAsync();

        var latestEmployees =
            await _context.Employees
                .OrderByDescending(x => x.Id)
                .Take(5)
                .ToListAsync();

        return Ok(new
        {
            TotalEmployees = totalEmployees,
            TotalDepartments = totalDepartments,
            LatestEmployees = latestEmployees
        });
    }
}
