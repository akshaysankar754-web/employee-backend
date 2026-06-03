using EmployeePortalAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeePortalAPI.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    public DbSet<Employee> Employees { get; set; }
}