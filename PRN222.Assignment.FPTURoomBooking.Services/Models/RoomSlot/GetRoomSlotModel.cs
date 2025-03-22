using PRN222.Assignment.FPTURoomBooking.Services.Utils;

namespace PRN222.Assignment.FPTURoomBooking.Services.Models.RoomSlot;

public class GetRoomSlotModel : PaginationParams
{
    public Guid? RoomId { get; set; }
    public Guid? BookingId { get; set; }
}