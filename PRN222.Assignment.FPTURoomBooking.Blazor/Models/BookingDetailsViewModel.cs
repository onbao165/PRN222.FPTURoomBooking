using PRN222.Assignment.FPTURoomBooking.Repositories.Models;

namespace PRN222.Assignment.FPTURoomBooking.Blazor.Models;

public class BookingDetailsViewModel
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public string AccountName { get; set; } = string.Empty;
    public DateTime BookingDate { get; set; }
    public BookingStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<BookingSlotViewModel> Slots { get; set; } = new();
}

public class BookingSlotViewModel
{
    public string RoomName { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}
