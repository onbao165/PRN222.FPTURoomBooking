using PRN222.Assignment.FPTURoomBooking.Repositories.Models;
using PRN222.Assignment.FPTURoomBooking.Services.Utils;

namespace PRN222.Assignment.FPTURoomBooking.Mvc.Models;

public class RoomSlotViewModel
{
    public Guid Id { get; set; }
    public Guid RoomId { get; set; }
    public string RoomName { get; set; } = string.Empty;
    public string CampusName { get; set; } = string.Empty;
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

public class RoomSlotListViewModel
{
    public PaginationResult<RoomSlotViewModel> RoomSlots { get; set; } = null!;
    public string? SearchTerm { get; set; }
    public string? OrderBy { get; set; }
    public bool IsDescending { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public Guid? RoomId { get; set; }
}
