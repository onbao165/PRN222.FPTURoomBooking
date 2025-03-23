using PRN222.Assignment.FPTURoomBooking.Services.Models.Department;

namespace PRN222.Assignment.FPTURoomBooking.Services.Models.Room;

public class RoomModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public Guid DepartmentId { get; set; }
    public DepartmentModel Department { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}