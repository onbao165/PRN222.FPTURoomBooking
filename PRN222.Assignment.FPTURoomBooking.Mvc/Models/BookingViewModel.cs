using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
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

    [Display(Name = "Booked By")] 
    public string AccountName { get; set; } = string.Empty;

    public Guid? ManagerId { get; set; }

    [Display(Name = "Approved By")] 
    public string? ManagerName { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // Single slot information
    public Guid RoomId { get; set; }
    [Display(Name = "Room")]
    public string RoomName { get; set; } = string.Empty;
    [Display(Name = "Campus")]
    public string CampusName { get; set; } = string.Empty;
    [Display(Name = "Start Time")]
    public DateTime StartTime { get; set; }
    [Display(Name = "End Time")]
    public DateTime EndTime { get; set; }
}

public class BookingListViewModel
{
    public PaginationResult<BookingViewModel> Bookings { get; set; } = null!;
    public string? SearchTerm { get; set; }
    public string? OrderBy { get; set; }
    public bool IsDescending { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
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

    [Required]
    public Guid CampusId { get; set; }

    [Required]
    public Guid RoomId { get; set; }

    [Required]
    [Display(Name = "Start Time")]
    public DateTime StartTime { get; set; }

    [Required]
    [Display(Name = "End Time")]
    public DateTime EndTime { get; set; }

    // For dropdowns in the view
    public IEnumerable<SelectListItem> Campuses { get; set; } = [];
    public IEnumerable<SelectListItem> Rooms { get; set; } = [];
}

public class EditBookingViewModel
{
    public Guid Id { get; set; }

    [Required]
    [Display(Name = "Booking Date")]
    [DataType(DataType.Date)]
    public DateTime BookingDate { get; set; }

    [Display(Name = "Status")] 
    public BookingStatus Status { get; set; }

    public Guid AccountId { get; set; }

    [Display(Name = "Booked By")] 
    public string AccountName { get; set; } = string.Empty;

    // Single slot information
    public Guid RoomId { get; set; }
    [Display(Name = "Room")]
    public string RoomName { get; set; } = string.Empty;
    [Display(Name = "Campus")]
    public string CampusName { get; set; } = string.Empty;
    [Display(Name = "Start Time")]
    public DateTime StartTime { get; set; }
    [Display(Name = "End Time")]
    public DateTime EndTime { get; set; }
}