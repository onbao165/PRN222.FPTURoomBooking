using PRN222.Assignment.FPTURoomBooking.Repositories.Models;

namespace PRN222.Assignment.FPTURoomBooking.Blazor.Models;

public class BookingDetailsViewModel
{
    public Guid Id { get; set; }
    public DateTime BookingDate { get; set; }
    public BookingStatus Status { get; set; }
    public string AccountName { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<BookingRoomSlotViewModel> RoomSlots { get; set; } = new();
}

public class BookingRoomSlotViewModel
{
    public string RoomName { get; set; } = string.Empty;
    public TimeSlot TimeSlot { get; set; }
}
