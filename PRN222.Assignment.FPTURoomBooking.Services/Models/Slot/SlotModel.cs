using PRN222.Assignment.FPTURoomBooking.Services.Models.Booking;
using PRN222.Assignment.FPTURoomBooking.Services.Models.Room;

namespace PRN222.Assignment.FPTURoomBooking.Services.Models.Slot;

public class SlotModel
{
    public Guid Id { get; set; }
    public Guid RoomId { get; set; } // RoomSlot belongs to a room
    public Guid BookingId { get; set; } // RoomSlot belongs to a booking
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public RoomModel Room { get; set; }
    public BookingModel Booking { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}