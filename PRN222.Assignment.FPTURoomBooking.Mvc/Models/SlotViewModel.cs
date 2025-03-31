using PRN222.Assignment.FPTURoomBooking.Services.Utils;

namespace PRN222.Assignment.FPTURoomBooking.Mvc.Models;

public class SlotViewModel
{
    public Guid Id { get; set; }
    public Guid RoomId { get; set; }
    public string RoomName { get; set; } = string.Empty;
    public string CampusName { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}

public class SlotListViewModel
{
    public PaginationResult<SlotViewModel> Slots { get; set; } = null!;
    public string? SearchTerm { get; set; }
    public string? OrderBy { get; set; }
    public bool IsDescending { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public Guid? RoomId { get; set; }
}
