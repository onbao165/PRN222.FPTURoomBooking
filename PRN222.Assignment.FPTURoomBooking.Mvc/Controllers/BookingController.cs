using System.Security.Claims;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using PRN222.Assignment.FPTURoomBooking.Mvc.Hubs;
using PRN222.Assignment.FPTURoomBooking.Mvc.Models;
using PRN222.Assignment.FPTURoomBooking.Repositories.Models;
using PRN222.Assignment.FPTURoomBooking.Services.Models.Booking;
using PRN222.Assignment.FPTURoomBooking.Services.Models.Campus;
using PRN222.Assignment.FPTURoomBooking.Services.Models.Room;
using PRN222.Assignment.FPTURoomBooking.Services.Models.Slot;
using PRN222.Assignment.FPTURoomBooking.Services.Services.Interfaces;
using PRN222.Assignment.FPTURoomBooking.Services.Utils;

namespace PRN222.Assignment.FPTURoomBooking.Mvc.Controllers;

[Authorize(Roles = "User")]
public class BookingController : Controller
{
    private readonly IBookingService _bookingService;
    private readonly ISlotService _slotService;
    private readonly IAccountService _accountService;
    private readonly IRoomService _roomService;
    private readonly ICampusService _campusService;
    private readonly IHubContext<MessageHub, IMessageHubClient> _hubContext;

    public BookingController(
        IBookingService bookingService,
        ISlotService slotService,
        IAccountService accountService,
        IRoomService roomService,
        ICampusService campusService,
        IHubContext<MessageHub, IMessageHubClient> hubContext)
    {
        _bookingService = bookingService;
        _slotService = slotService;
        _accountService = accountService;
        _roomService = roomService;
        _campusService = campusService;
        _hubContext = hubContext;
    }

    [HttpGet]
    public async Task<IActionResult> Index(BookingListViewModel model)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(currentUserId))
        {
            return Challenge();
        }

        ViewBag.Statuses = new SelectList(
            Enum.GetValues(typeof(BookingStatus))
                .Cast<BookingStatus>()
                .Select(s => new { Id = s, Name = s.ToString() }),
            "Id", "Name", model.Status);

        var bookingModel = new GetBookingModel
        {
            PageNumber = model.PageNumber,
            PageSize = model.PageSize,
            SearchTerm = model.SearchTerm ?? string.Empty,
            OrderBy = model.OrderBy ?? "bookingDate",
            IsDescending = model.IsDescending,
            Status = model.Status,
            AccountId = Guid.Parse(currentUserId),
            StartDate = model.StartDate,
            EndDate = model.EndDate
        };

        var result = await _bookingService.GetPagedAsync(bookingModel);

        if (!result.IsSuccess || result.Data == null)
        {
            return View("Error", new ErrorViewModel
            {
                Message = result.Error,
                RequestId = HttpContext.TraceIdentifier
            });
        }

        var bookings = new List<BookingViewModel>();
        foreach (var booking in result.Data.Items)
        {
            var viewModel = booking.Adapt<BookingViewModel>();
            viewModel.AccountName = booking.Account.FullName;

            if (booking.ManagerId.HasValue)
            {
                var managerResult = await _accountService.GetAsync(booking.ManagerId.Value);
                if (managerResult is { IsSuccess: true, Data: not null })
                {
                    viewModel.ManagerName = managerResult.Data.FullName;
                }
            }

            // Get the single slot for this booking
            var slot = await _slotService.GetByBookingIdAsync(booking.Id);
            if (slot is { IsSuccess: true, Data: not null })
            {
                var slotData = slot.Data;
                viewModel.RoomId = slotData.RoomId;
                viewModel.RoomName = slotData.Room.Name;
                viewModel.CampusName = slotData.Room.Campus.Name;
                viewModel.StartTime = slotData.StartTime;
                viewModel.EndTime = slotData.EndTime;
            }

            bookings.Add(viewModel);
        }

        var resultModel = new BookingListViewModel
        {
            Bookings = new PaginationResult<BookingViewModel>(bookings, result.Data.TotalItems, result.Data.PageNumber,
                result.Data.PageSize),
            SearchTerm = model.SearchTerm,
            OrderBy = model.OrderBy,
            IsDescending = model.IsDescending,
            PageNumber = model.PageNumber,
            PageSize = model.PageSize,
            Status = model.Status,
            StartDate = model.StartDate,
            EndDate = model.EndDate
        };

        return View(resultModel);
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(currentUserId))
        {
            return Challenge();
        }

        var bookingResult = await _bookingService.GetAsync(id);

        if (!bookingResult.IsSuccess || bookingResult.Data == null)
        {
            return NotFound(bookingResult.Error);
        }

        if (bookingResult.Data.AccountId != Guid.Parse(currentUserId))
        {
            return Forbid();
        }

        var booking = bookingResult.Data.Adapt<BookingViewModel>();
        booking.AccountName = bookingResult.Data.Account.FullName;

        if (bookingResult.Data.ManagerId.HasValue)
        {
            var managerResult = await _accountService.GetAsync(bookingResult.Data.ManagerId.Value);
            if (managerResult is { IsSuccess: true, Data: not null })
            {
                booking.ManagerName = managerResult.Data.FullName;
            }
        }

        // Get the single slot for this booking
        var slot = await _slotService.GetByBookingIdAsync(booking.Id, booking.Status == BookingStatus.Cancelled);
        if (slot is { IsSuccess: true, Data: not null })
        {
            var slotData = slot.Data;
            booking.RoomId = slotData.RoomId;
            booking.RoomName = slotData.Room.Name;
            booking.CampusName = slotData.Room.Campus.Name;
            booking.StartTime = slotData.StartTime;
            booking.EndTime = slotData.EndTime;
        }

        return View(booking);
    }

    [HttpGet]
    public async Task<IActionResult> Create(Guid? roomId = null)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(currentUserId))
        {
            return Challenge();
        }

        var model = new CreateBookingViewModel
        {
            BookingDate = DateTime.Today.AddDays(1).Date
        };

        if (roomId.HasValue && roomId.Value != Guid.Empty)
        {
            var roomResult = await _roomService.GetAsync(roomId.Value);
            if (roomResult is { IsSuccess: true, Data: not null })
            {
                model.RoomId = roomId.Value;
                model.CampusId = roomResult.Data.Campus.Id;
            }
        }

        await PopulateDropdowns(model);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateBookingViewModel model)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var currentDepartmentId = User.FindFirstValue("DepartmentId");
        if (string.IsNullOrEmpty(currentUserId) || string.IsNullOrEmpty(currentDepartmentId))
        {
            return Challenge();
        }
        if (!ModelState.IsValid)
        {
            await PopulateDropdowns(model);
            return View(model);
        }

        // Validate booking date
        if (model.BookingDate.Date < DateTime.Today.AddDays(1).Date)
        {
            ModelState.AddModelError(nameof(model.BookingDate), "Booking date must be at least one day from today");
            await PopulateDropdowns(model);
            return View(model);
        }

        // Ensure StartTime and EndTime use the same date as BookingDate
        model.StartTime = model.BookingDate.Date.Add(model.StartTime.TimeOfDay);
        model.EndTime = model.BookingDate.Date.Add(model.EndTime.TimeOfDay);

        // Validate time range
        if (model.StartTime >= model.EndTime)
        {
            ModelState.AddModelError(nameof(model.StartTime), "Start time must be before end time");
            await PopulateDropdowns(model);
            return View(model);
        }

        // Validate booking hours (6 AM - 10 PM)
        var earliestTime = model.BookingDate.Date.AddHours(6); // 6 AM
        var latestTime = model.BookingDate.Date.AddHours(22);  // 10 PM

        if (model.StartTime < earliestTime || model.EndTime > latestTime)
        {
            ModelState.AddModelError(string.Empty, "Booking hours must be between 6:00 AM and 10:00 PM");
            await PopulateDropdowns(model);
            return View(model);
        }

        // Validate minimum duration (30 minutes)
        if ((model.EndTime - model.StartTime).TotalMinutes < 30)
        {
            ModelState.AddModelError(string.Empty, "Booking duration must be at least 30 minutes");
            await PopulateDropdowns(model);
            return View(model);
        }

        // Check for time slot conflicts
        var existingSlots = await _slotService.GetByRoomAndDateAsync(
            model.RoomId,
            model.BookingDate);

        if (existingSlots is { IsSuccess: true, Data: not null })
        {
            foreach (var existingSlot in existingSlots.Data)
            {

                // Check if the new slot overlaps with any existing slot
                if (!DoTimeRangesOverlap(
                        model.StartTime, model.EndTime,
                        existingSlot.StartTime, existingSlot.EndTime)) continue;
                ModelState.AddModelError(string.Empty, 
                    $"This time slot conflicts with an existing booking ({existingSlot.StartTime:HH:mm} - {existingSlot.EndTime:HH:mm})");
                await PopulateDropdowns(model);
                return View(model);
            }
        }

        // Create booking model
        var booking = new BookingModel
        {
            Id = Guid.NewGuid(),
            BookingDate = model.BookingDate.Date, // Ensure we store just the date
            Status = BookingStatus.Pending,
            AccountId = Guid.Parse(currentUserId)
        };

        // Create single slot model
        var slot = new SlotModel
        {
            RoomId = model.RoomId,
            BookingId = booking.Id,
            StartTime = model.StartTime, // Already normalized to booking date
            EndTime = model.EndTime     // Already normalized to booking date
        };

        // Create booking with single slot
        var result = await _bookingService.CreateBookingWithSlots(booking, slot);

        if (result is { IsSuccess: true, Data: not null })
        {
            // Send notification to managers
            await _hubContext.Clients.Group(currentDepartmentId).ReceiveNewBooking();
            return RedirectToAction(nameof(Details), new { id = result.Data.Id });
        }

        ModelState.AddModelError(string.Empty, result.Error ?? "Failed to create booking");
        await PopulateDropdowns(model);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(Guid id)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var currentDepartmentId = User.FindFirstValue("DepartmentId");
        if (string.IsNullOrEmpty(currentUserId) || string.IsNullOrEmpty(currentDepartmentId))
        {
            return Challenge();
        }

        var bookingResult = await _bookingService.GetAsync(id);
        if (!bookingResult.IsSuccess || bookingResult.Data == null)
        {
            return NotFound(bookingResult.Error);
        }

        // Check if user has permission to cancel this booking
        if (bookingResult.Data.AccountId != Guid.Parse(currentUserId))
        {
            return Forbid();
        }

        // Only pending bookings can be cancelled
        if (bookingResult.Data.Status != BookingStatus.Pending)
        {
            TempData["ErrorMessage"] = "Only pending bookings can be cancelled";
            return RedirectToAction(nameof(Details), new { id });
        }

        // Update booking status to cancelled
        bookingResult.Data.Status = BookingStatus.Cancelled;
        var updateResult = await _bookingService.UpdateAsync(bookingResult.Data);

        if (!updateResult.IsSuccess)
        {
            TempData["ErrorMessage"] = updateResult.Error ?? "Failed to cancel booking";
            return RedirectToAction(nameof(Details), new { id });
        }

        // Delete slots associated with this booking
        var slot = await _slotService.GetByBookingIdAsync(id);
        if (slot is { IsSuccess: true, Data: not null })
        {
            await _slotService.DeleteAsync(slot.Data.Id);
        }

        TempData["SuccessMessage"] = "Booking cancelled successfully";
        // Send notification to managers
        await _hubContext.Clients.Group(currentDepartmentId).ReceiveBookingStatusUpdate();
        return RedirectToAction(nameof(Details), new { id });
    }

    private async Task PopulateDropdowns(CreateBookingViewModel model)
    {
        // Get campuses
        var campusesResult = await _campusService.GetPagedAsync(new GetCampusModel { PageSize = 100 });
        model.Campuses = campusesResult is { IsSuccess: true, Data: not null }
            ? campusesResult.Data.Items.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
            : [];

        // Get rooms for selected campus
        if (model.CampusId != Guid.Empty)
        {
            var roomsResult = await _roomService.GetPagedAsync(new GetRoomModel
            {
                PageSize = 100,
                CampusId = model.CampusId
            });
            model.Rooms = roomsResult is { IsSuccess: true, Data: not null }
                ? roomsResult.Data.Items.Select(r => new SelectListItem { Value = r.Id.ToString(), Text = r.Name })
                : [];
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetRooms(Guid campusId)
    {
        var roomsResult = await _roomService.GetPagedAsync(new GetRoomModel
        {
            PageSize = 100,
            CampusId = campusId
        });

        var rooms = roomsResult is { IsSuccess: true, Data: not null }
            ? roomsResult.Data.Items.Select(r => new { id = r.Id, name = r.Name })
            : Enumerable.Empty<object>();

        return Json(rooms);
    }

    [HttpGet]
    public async Task<IActionResult> GetBookedSlots(Guid roomId, DateTime date)
    {
        var slots = await _slotService.GetByRoomAndDateAsync(roomId, date);
        if (slots is { IsSuccess: true, Data: not null })
        {
            return PartialView("_BookedSlotsPartial", slots.Data.Select(s => s.Adapt<SlotViewModel>()));
        }
        return PartialView("_BookedSlotsPartial", Enumerable.Empty<SlotViewModel>());
    }

    private static bool DoTimeRangesOverlap(DateTime start1, DateTime end1, DateTime start2, DateTime end2)
    {
        return start1 < end2 && start2 < end1;
    }
}
