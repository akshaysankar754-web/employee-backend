using EmployeePortalAPI.Data;
using EmployeePortalAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeePortalAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EmployeeController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public EmployeeController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET ALL EMPLOYEES
    [HttpGet]
    public async Task<IActionResult> GetAllEmployees()
    {
        var employees =
            await _context.Employees.ToListAsync();

        return Ok(employees);
    }

    // GET EMPLOYEE BY ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEmployeeById(int id)
    {
        var employee =
            await _context.Employees.FindAsync(id);

        if (employee == null)
        {
            return NotFound("Employee not found");
        }

        return Ok(employee);
    }

    // ADD EMPLOYEE
    [HttpPost]
    public async Task<IActionResult> AddEmployee(Employee employee)
    {
        _context.Employees.Add(employee);

        await _context.SaveChangesAsync();

        return Ok(new
        {
            Message = "Employee Added Successfully",
            Employee = employee
        });
    }

    // UPDATE EMPLOYEE
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployee(
        int id,
        Employee updatedEmployee)
    {
        var employee =
            await _context.Employees.FindAsync(id);

        if (employee == null)
        {
            return NotFound("Employee not found");
        }

        employee.FirstName =
            updatedEmployee.FirstName;

        employee.LastName =
            updatedEmployee.LastName;

        employee.Email =
            updatedEmployee.Email;

        employee.Phone =
            updatedEmployee.Phone;

        employee.Department =
            updatedEmployee.Department;

        employee.Position =
            updatedEmployee.Position;

        employee.Salary =
            updatedEmployee.Salary;

        employee.DateOfJoining =
            updatedEmployee.DateOfJoining;

        await _context.SaveChangesAsync();

        return Ok(new
        {
            Message = "Employee Updated Successfully",
            Employee = employee
        });
    }

    // DELETE EMPLOYEE
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var employee =
            await _context.Employees.FindAsync(id);

        if (employee == null)
        {
            return NotFound("Employee not found");
        }

        _context.Employees.Remove(employee);

        await _context.SaveChangesAsync();

        return Ok(new
        {
            Message = "Employee Deleted Successfully"
        });
    }
}