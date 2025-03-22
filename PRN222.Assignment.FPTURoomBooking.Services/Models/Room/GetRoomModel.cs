using PRN222.Assignment.FPTURoomBooking.Services.Utils;

namespace PRN222.Assignment.FPTURoomBooking.Services.Models.Room;

public class GetRoomModel : PaginationParams
{
    public Guid? DepartmentId { get; set; }
}