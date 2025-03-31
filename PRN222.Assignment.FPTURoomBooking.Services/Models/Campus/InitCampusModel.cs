using System.ComponentModel.DataAnnotations;

namespace PRN222.Assignment.FPTURoomBooking.Services.Models.Campus;

public class InitCampusModel
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(255, ErrorMessage = "Name cannot exceed 255 characters")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "Address is required")]
    [StringLength(1000, ErrorMessage = "Address cannot exceed 1000 characters")]
    public required string Address { get; set; }
}