using PRN222.Assignment.FPTURoomBooking.Services.Models.Campus;

namespace PRN222.Assignment.FPTURoomBooking.Services.Models.Department;

public class DepartmentModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public Guid CampusId { get; set; } // Department belongs to a campus
    public CampusModel Campus { get; set; }
}