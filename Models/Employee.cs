namespace EmployeePortalAPI.Models;

public class Employee
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string Phone { get; set; }

    public string Department { get; set; }

    public string Designation { get; set; }

    public decimal Salary { get; set; }

    public DateTime DateOfJoining { get; set; }
}