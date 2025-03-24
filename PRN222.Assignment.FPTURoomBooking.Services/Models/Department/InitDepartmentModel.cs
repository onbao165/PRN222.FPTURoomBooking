using System.ComponentModel.DataAnnotations;

namespace PRN222.Assignment.FPTURoomBooking.Services.Models.Department;

public class InitDepartmentModel
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(255, ErrorMessage = "Name cannot exceed 255 characters")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "Description is required")]
    [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
    public required string Description { get; set; }

    [Required(ErrorMessage = "Campus is required")]
    public required Guid CampusId { get; set; }
}