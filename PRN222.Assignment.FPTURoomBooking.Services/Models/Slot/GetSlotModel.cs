using PRN222.Assignment.FPTURoomBooking.Services.Utils;

namespace PRN222.Assignment.FPTURoomBooking.Services.Models.Slot;

public class GetSlotModel : PaginationParams
{
    public Guid? RoomId { get; set; }
    public Guid? BookingId { get; set; }
    public bool IncludeDeleted { get; set; } 
}