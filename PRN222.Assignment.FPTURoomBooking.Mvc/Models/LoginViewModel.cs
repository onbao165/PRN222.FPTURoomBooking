using System.ComponentModel.DataAnnotations;

namespace PRN222.Assignment.FPTURoomBooking.Mvc.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; }
    
    public string? ReturnUrl { get; set; }
} 