using System.ComponentModel.DataAnnotations;
using PRN222.Assignment.FPTURoomBooking.Repositories.Models;

namespace PRN222.Assignment.FPTURoomBooking.Services.Models.Account;

public class InitAccountModel
{
    [Required(ErrorMessage = "Full name is required")]
    [StringLength(255, ErrorMessage = "Full name cannot exceed 255 characters")]
    public required string FullName { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Username is required")]
    [StringLength(255, ErrorMessage = "Username cannot exceed 255 characters")]
    [RegularExpression(@"^[a-zA-Z0-9_]+$",
        ErrorMessage = "Username can only contain letters, numbers, and underscores")]
    public required string Username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [StringLength(255, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 255 characters")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$",
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, and one number")]
    public required string Password { get; set; }

    [Required(ErrorMessage = "Role is required")]
    [EnumDataType(typeof(AccountRole), ErrorMessage = "Invalid role")]
    public AccountRole Role { get; set; }

    [Required(ErrorMessage = "Department is required")]
    public Guid DepartmentId { get; set; }
}