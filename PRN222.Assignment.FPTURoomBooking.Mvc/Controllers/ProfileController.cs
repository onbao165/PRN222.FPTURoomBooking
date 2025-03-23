using System.Security.Claims;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN222.Assignment.FPTURoomBooking.Mvc.Models;
using PRN222.Assignment.FPTURoomBooking.Services.Models.Booking;
using PRN222.Assignment.FPTURoomBooking.Services.Models.RoomSlot;
using PRN222.Assignment.FPTURoomBooking.Services.Services.Interfaces;

namespace PRN222.Assignment.FPTURoomBooking.Mvc.Controllers;

[Authorize(Roles = "User")]
public class ProfileController : Controller
{
    private readonly IAccountService _accountService;
    private readonly IBookingService _bookingService;
    private readonly IRoomSlotService _roomSlotService;
    private readonly IDepartmentService _departmentService;

    public ProfileController(
        IAccountService accountService,
        IBookingService bookingService,
        IRoomSlotService roomSlotService,
        IDepartmentService departmentService)
    {
        _accountService = accountService;
        _bookingService = bookingService;
        _roomSlotService = roomSlotService;
        _departmentService = departmentService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(currentUserId))
        {
            return Challenge();
        }

        var accountResult = await _accountService.GetAsync(Guid.Parse(currentUserId));
        if (!accountResult.IsSuccess || accountResult.Data == null)
        {
            return View("Error", new ErrorViewModel
            {
                Message = accountResult.Error,
                RequestId = HttpContext.TraceIdentifier
            });
        }

        // Create profile view model
        var profileViewModel = accountResult.Data.Adapt<ProfileViewModel>();

        // Get department name if applicable
        if (accountResult.Data.DepartmentId.HasValue)
        {
            var departmentResult = await _departmentService.GetAsync(accountResult.Data.DepartmentId.Value);
            if (departmentResult is { IsSuccess: true, Data: not null })
            {
                profileViewModel.DepartmentName = departmentResult.Data.Name;
            }
        }

        // Get recent bookings
        var bookingResult = await _bookingService.GetPagedAsync(new GetBookingModel
        {
            AccountId = Guid.Parse(currentUserId),
            PageNumber = 1,
            PageSize = 5,
            OrderBy = "bookingDate",
            IsDescending = true
        });

        if (!bookingResult.IsSuccess || bookingResult.Data == null) return View(profileViewModel);
        // Map bookings to view models
        var bookings = new List<BookingViewModel>();
        foreach (var booking in bookingResult.Data.Items)
        {
            var bookingViewModel = booking.Adapt<BookingViewModel>();

            // Add account name
            bookingViewModel.AccountName = booking.Account.FullName;

            // Get room slots for this booking
            var roomSlotsModel = new GetRoomSlotModel
            {
                BookingId = booking.Id,
                PageSize = 100 // Get all room slots for this booking
            };

            var roomSlotsResult = await _roomSlotService.GetPagedAsync(roomSlotsModel);
            if (roomSlotsResult is { IsSuccess: true, Data: not null })
            {
                foreach (var slot in roomSlotsResult.Data.Items)
                {
                    var roomSlotViewModel = slot.Adapt<RoomSlotViewModel>();

                    roomSlotViewModel.RoomName = slot.Room.Name;

                    roomSlotViewModel.DepartmentName = slot.Room.Department.Name;

                    roomSlotViewModel.CampusName = slot.Room.Department.Campus.Name;

                    bookingViewModel.RoomSlots.Add(roomSlotViewModel);
                }
            }

            bookings.Add(bookingViewModel);
        }

        profileViewModel.RecentBookings = new Services.Utils.PaginationResult<BookingViewModel>(bookings,
            bookingResult.Data.TotalItems, bookingResult.Data.PageNumber, bookingResult.Data.PageSize);
        return View(profileViewModel);
    }
}