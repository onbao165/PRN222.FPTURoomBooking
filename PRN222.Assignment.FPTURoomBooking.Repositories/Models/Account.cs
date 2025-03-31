using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using PRN222.Assignment.FPTURoomBooking.Repositories.Models.Base;

namespace PRN222.Assignment.FPTURoomBooking.Repositories.Models;

public class Account : AuditableEntity
{
    [MaxLength(255)] public string Username { get; set; } = null!;
    [MaxLength(255)] public string Password { get; set; } = null!;
    [MaxLength(255)] public string Email { get; set; } = null!;
    [MaxLength(255)] public string? FullName { get; set; }
    public AccountRole Role { get; set; }
    [ForeignKey(nameof(Department))] public Guid? DepartmentId { get; set; } // Manager belongs to a department
    public virtual Department? Department { get; set; }
    public virtual ICollection<Booking> Bookings { get; set; } = [];

    public static Expression<Func<Account, object>> GetSortValue(string orderBy)
    {
        return orderBy switch
        {
            "username" => account => account.Username,
            "email" => account => account.Email,
            "role" => account => account.Role,
            "id" => account => account.Id,
            _ => account => account.UpdatedAt ?? account.CreatedAt
        };
    }
}

public enum AccountRole
{
    Manager = 1,
    User = 2,
    Admin = 3
}