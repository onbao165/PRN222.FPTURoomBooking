using System.ComponentModel.DataAnnotations;

namespace PRN222.Assignment.FPTURoomBooking.Blazor.Models;

public class LoginModel
{
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}