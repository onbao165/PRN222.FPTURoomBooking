using PRN222.Assignment.FPTURoomBooking.Repositories.Models;
using PRN222.Assignment.FPTURoomBooking.Services.Models.Booking;
using PRN222.Assignment.FPTURoomBooking.Services.Models.Room;

namespace PRN222.Assignment.FPTURoomBooking.Services.Models.RoomSlot;

public class RoomSlotModel
{
    public Guid Id { get; set; }
    public Guid RoomId { get; set; } // RoomSlot belongs to a room
    public Guid BookingId { get; set; } // RoomSlot belongs to a booking
    public TimeSlot TimeSlot { get; set; }
    public RoomModel Room { get; set; }
    public BookingModel Booking { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}