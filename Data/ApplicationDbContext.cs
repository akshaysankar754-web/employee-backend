using Microsoft.EntityFrameworkCore;
using EmployeePortalAPI.Models;

namespace EmployeePortalAPI.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<Employee> Employees => Set<Employee>();
}
