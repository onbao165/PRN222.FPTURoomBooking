using PRN222.Assignment.FPTURoomBooking.Services.Models.Campus;
using PRN222.Assignment.FPTURoomBooking.Services.Models.Department;

namespace PRN222.Assignment.FPTURoomBooking.Services.Models.Room;

public class RoomModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public Guid CampusId { get; set; } // Room belongs to a campus
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public CampusModel Campus { get; set; }
}