using PRN222.Assignment.FPTURoomBooking.Repositories.Models;
using PRN222.Assignment.FPTURoomBooking.Services.Models.Department;

namespace PRN222.Assignment.FPTURoomBooking.Services.Models.Account;

public class AccountModel
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public AccountRole Role { get; set; }
    public Guid? DepartmentId { get; set; } // Manager belongs to a department
    public DepartmentModel? Department { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}