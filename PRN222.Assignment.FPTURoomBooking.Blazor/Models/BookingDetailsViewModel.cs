using PRN222.Assignment.FPTURoomBooking.Repositories.Models;

namespace PRN222.Assignment.FPTURoomBooking.Blazor.Models;

public class BookingDetailsViewModel
{
    public Guid Id { get; set; }
    public DateTime BookingDate { get; set; }
    public BookingStatus Status { get; set; }
    
    public Guid AccountId { get; set; }
    public string AccountName { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<BookingRoomSlotViewModel> RoomSlots { get; set; } = [];
}

public class BookingRoomSlotViewModel
{
    public string RoomName { get; set; } = string.Empty;
    public TimeSlot TimeSlot { get; set; }
    public string TimeSlotText => TimeSlot switch
    {
        TimeSlot.Slot1 => "7:00 - 9:15",
        TimeSlot.Slot2 => "9:30 - 11:45",
        TimeSlot.Slot3 => "12:30 - 14:45",
        TimeSlot.Slot4 => "15:00 - 17:15",
        TimeSlot.Slot5 => "17:45 - 20:00",
        _ => "Unknown"
    };
}
