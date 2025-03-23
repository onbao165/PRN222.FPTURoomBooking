using System.ComponentModel.DataAnnotations;
using PRN222.Assignment.FPTURoomBooking.Repositories.Models;
using PRN222.Assignment.FPTURoomBooking.Services.Utils;

namespace PRN222.Assignment.FPTURoomBooking.Mvc.Models;

public class BookingViewModel
{
    public Guid Id { get; set; }

    [Display(Name = "Booking Date")]
    [DataType(DataType.Date)]
    public DateTime BookingDate { get; set; }

    public BookingStatus Status { get; set; }

    public string StatusName => Status.ToString();

    public Guid AccountId { get; set; }

    [Display(Name = "Booked By")] public string AccountName { get; set; } = string.Empty;

    public Guid? ManagerId { get; set; }

    [Display(Name = "Approved By")] public string? ManagerName { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public List<RoomSlotViewModel> RoomSlots { get; set; } = new();
}

public class BookingListViewModel
{
    public PaginationResult<BookingViewModel> Bookings { get; set; } = null!;

    public string? SearchTerm { get; set; }

    public string? OrderBy { get; set; }

    public bool IsDescending { get; set; }

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public Guid? AccountId { get; set; }

    public Guid? ManagerId { get; set; }

    public BookingStatus? Status { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }
}

public class CreateBookingViewModel
{
    [Required]
    [Display(Name = "Booking Date")]
    [DataType(DataType.Date)]
    public DateTime BookingDate { get; set; }

    // Filter properties
    public Guid? CampusId { get; set; }
    public Guid? DepartmentId { get; set; }
    public Guid? RoomId { get; set; }

    // Selected room slots
    [Required(ErrorMessage = "Please select at least one room slot")]
    public List<RoomSlotSelectionViewModel> SelectedRoomSlots { get; set; } = new();
    
    // Available room slots for selection
    public List<RoomSlotViewModel> AvailableRoomSlots { get; set; } = new();
}

public class RoomSlotSelectionViewModel
{
    public Guid RoomId { get; set; }
    public TimeSlot TimeSlot { get; set; }
}

public class EditBookingViewModel
{
    public Guid Id { get; set; }

    [Required]
    [Display(Name = "Booking Date")]
    [DataType(DataType.Date)]
    public DateTime BookingDate { get; set; }

    [Display(Name = "Status")] public BookingStatus Status { get; set; }

    [Display(Name = "Room Slots")] public List<RoomSlotViewModel> RoomSlots { get; set; } = new();

    public Guid AccountId { get; set; }

    [Display(Name = "Booked By")] public string AccountName { get; set; } = string.Empty;

    public bool IsManager { get; set; }

    [Display(Name = "Reason for Rejection")]
    [StringLength(500, ErrorMessage = "Reason cannot exceed 500 characters")]
    public string? RejectionReason { get; set; }
}
