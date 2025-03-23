using System.Security.Claims;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PRN222.Assignment.FPTURoomBooking.Mvc.Models;
using PRN222.Assignment.FPTURoomBooking.Repositories.Models;
using PRN222.Assignment.FPTURoomBooking.Services.Models.Booking;
using PRN222.Assignment.FPTURoomBooking.Services.Models.Campus;
using PRN222.Assignment.FPTURoomBooking.Services.Models.Department;
using PRN222.Assignment.FPTURoomBooking.Services.Models.Room;
using PRN222.Assignment.FPTURoomBooking.Services.Models.RoomSlot;
using PRN222.Assignment.FPTURoomBooking.Services.Services.Interfaces;
using PRN222.Assignment.FPTURoomBooking.Services.Utils;

namespace PRN222.Assignment.FPTURoomBooking.Mvc.Controllers;

[Authorize]
public class BookingController : Controller
{
    private readonly IBookingService _bookingService;
    private readonly IRoomSlotService _roomSlotService;
    private readonly IAccountService _accountService;
    private readonly IRoomService _roomService;
    private readonly IDepartmentService _departmentService;
    private readonly ICampusService _campusService;

    public BookingController(
        IBookingService bookingService,
        IRoomSlotService roomSlotService,
        IAccountService accountService,
        IRoomService roomService,
        IDepartmentService departmentService,
        ICampusService campusService)
    {
        _bookingService = bookingService;
        _roomSlotService = roomSlotService;
        _accountService = accountService;
        _roomService = roomService;
        _departmentService = departmentService;
        _campusService = campusService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(BookingListViewModel model)
    {
        // Get current user information
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(currentUserId))
        {
            return Challenge();
        }

        // Status list for dropdown
        ViewBag.Statuses = new SelectList(
            Enum.GetValues(typeof(BookingStatus))
                .Cast<BookingStatus>()
                .Select(s => new { Id = s, Name = s.ToString() }),
            "Id", "Name", model.Status);

        // Create filter model
        var bookingModel = new GetBookingModel
        {
            PageNumber = model.PageNumber,
            PageSize = model.PageSize,
            SearchTerm = model.SearchTerm ?? string.Empty,
            OrderBy = model.OrderBy ?? "bookingDate",
            IsDescending = model.IsDescending,
            Status = model.Status,
            AccountId = Guid.Parse(currentUserId),
            // Add date range filtering
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

        // Map bookings to view models
        var bookings = new List<BookingViewModel>();
        foreach (var booking in result.Data.Items)
        {
            var viewModel = booking.Adapt<BookingViewModel>();

            // Add account name if available
            viewModel.AccountName = booking.Account.FullName;

            // Add manager account name if available
            if (booking.ManagerId.HasValue)
            {
                var managerResult = await _accountService.GetAsync(booking.ManagerId.Value);
                if (managerResult is { IsSuccess: true, Data: not null })
                {
                    viewModel.ManagerName = managerResult.Data.FullName;
                }
            }

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

                    viewModel.RoomSlots.Add(roomSlotViewModel);
                }
            }

            bookings.Add(viewModel);
        }

        var resultModel = new BookingListViewModel
        {
            Bookings = new PaginationResult<BookingViewModel>(bookings, result.Data.PageNumber, result.Data.PageSize,
                result.Data.TotalItems),
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

        // Check if user has permission to view this booking
        if (bookingResult.Data.AccountId != Guid.Parse(currentUserId))
        {
            return Forbid();
        }

        var booking = bookingResult.Data.Adapt<BookingViewModel>();

        // Add account name if available
        booking.AccountName = bookingResult.Data.Account.FullName;

        // Add manager account name if available
        if (bookingResult.Data.ManagerId.HasValue)
        {
            var managerResult = await _accountService.GetAsync(bookingResult.Data.ManagerId.Value);
            if (managerResult is { IsSuccess: true, Data: not null })
            {
                booking.ManagerName = managerResult.Data.FullName;
            }
        }

        // Get room slots for this booking
        var roomSlotsModel = new GetRoomSlotModel
        {
            BookingId = booking.Id,
            PageSize = 100, // Get all room slots for this booking
            IncludeDeleted = bookingResult.Data.Status is BookingStatus.Cancelled or BookingStatus.Rejected
        };

        var roomSlotsResult = await _roomSlotService.GetPagedAsync(roomSlotsModel);
        if (roomSlotsResult is not { IsSuccess: true, Data: not null }) return View(booking);
        foreach (var slot in roomSlotsResult.Data.Items)
        {
            var roomSlotViewModel = slot.Adapt<RoomSlotViewModel>();

            roomSlotViewModel.RoomName = slot.Room.Name;

            roomSlotViewModel.DepartmentName = slot.Room.Department.Name;

            roomSlotViewModel.CampusName = slot.Room.Department.Campus.Name;

            booking.RoomSlots.Add(roomSlotViewModel);
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

        // If roomId is provided, pre-fill filter options
        if (roomId.HasValue && roomId.Value != Guid.Empty)
        {
            var roomResult = await _roomService.GetAsync(roomId.Value);
            if (roomResult is { IsSuccess: true, Data: not null })
            {
                model.RoomId = roomId.Value;
                model.DepartmentId = roomResult.Data.Department.Id;
                model.CampusId = roomResult.Data.Department.Campus.Id;
            }
        }

        await PopulateAvailableRoomSlots(model);
        await PopulateDropdowns(model.CampusId, model.DepartmentId);

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateBookingViewModel model)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(currentUserId))
        {
            return Challenge();
        }

        if (!ModelState.IsValid)
        {
            await PopulateAvailableRoomSlots(model);
            await PopulateDropdowns(model.CampusId, model.DepartmentId);
            return View(model);
        }

        // Validate booking date
        if (model.BookingDate.Date < DateTime.Today.AddDays(1).Date)
        {
            ModelState.AddModelError(nameof(model.BookingDate), "Booking date must be at least one day from today");
            await PopulateAvailableRoomSlots(model);
            await PopulateDropdowns(model.CampusId, model.DepartmentId);
            return View(model);
        }

        var selectedSlots = model.SelectedRoomSlots.Where(s => s.RoomId != Guid.Empty).ToList();

        if (selectedSlots.Count == 0)
        {
            ModelState.AddModelError(nameof(model.SelectedRoomSlots), "Please select at least one room slot");
            await PopulateAvailableRoomSlots(model);
            await PopulateDropdowns(model.CampusId, model.DepartmentId);
            return View(model);
        }

        // Create booking model
        var booking = new BookingModel
        {
            Id = Guid.NewGuid(),
            BookingDate = model.BookingDate,
            Status = BookingStatus.Pending,
            AccountId = Guid.Parse(currentUserId)
        };

        // Create room slots models
        var roomSlots = selectedSlots.Select(selection => new RoomSlotModel
        {
            RoomId = selection.RoomId,
            BookingId = booking.Id,
            TimeSlot = selection.TimeSlot
        }).ToList();

        // Create booking with room slots in a single transaction
        var result = await _bookingService.CreateBookingWithRoomSlots(booking, roomSlots);

        if (result is { IsSuccess: true, Data: not null })
            return RedirectToAction(nameof(Details), new { id = result.Data.Id });
        ModelState.AddModelError(string.Empty, result.Error ?? "Failed to create booking");
        await PopulateAvailableRoomSlots(model);
        await PopulateDropdowns(model.CampusId, model.DepartmentId);
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
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

        // Check if user has permission to edit this booking
        if (bookingResult.Data.AccountId != Guid.Parse(currentUserId))
        {
            return Forbid();
        }

        // Check if booking is in a state that can be edited
        if (bookingResult.Data.Status != BookingStatus.Pending)
        {
            TempData["ErrorMessage"] = "Only pending bookings can be edited";
            return RedirectToAction(nameof(Details), new { id });
        }

        var model = bookingResult.Data.Adapt<EditBookingViewModel>();
        model.IsManager = false;

        // Add account name if available
        model.AccountName = bookingResult.Data.Account.FullName;

        // Get room slots for this booking
        var roomSlotsModel = new GetRoomSlotModel
        {
            BookingId = model.Id,
            PageSize = 100, // Get all room slots for this booking
            // Include deleted room slots if booking is cancelled or rejected
            IncludeDeleted = bookingResult.Data.Status is BookingStatus.Cancelled or BookingStatus.Rejected
        };

        var roomSlotsResult = await _roomSlotService.GetPagedAsync(roomSlotsModel);
        if (roomSlotsResult is not { IsSuccess: true, Data: not null }) return View(model);
        foreach (var slot in roomSlotsResult.Data.Items)
        {
            var roomSlotViewModel = slot.Adapt<RoomSlotViewModel>();

            roomSlotViewModel.RoomName = slot.Room.Name;

            roomSlotViewModel.DepartmentName = slot.Room.Department.Name;

            roomSlotViewModel.CampusName = slot.Room.Department.Campus.Name;

            model.RoomSlots.Add(roomSlotViewModel);
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditBookingViewModel model)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(currentUserId))
        {
            return Challenge();
        }

        var bookingResult = await _bookingService.GetAsync(model.Id);
        if (!bookingResult.IsSuccess || bookingResult.Data == null)
        {
            return NotFound(bookingResult.Error);
        }

        // Check if user has permission to edit this booking
        if (bookingResult.Data.AccountId != Guid.Parse(currentUserId))
        {
            return Forbid();
        }

        // Only pending bookings can be edited
        if (bookingResult.Data.Status != BookingStatus.Pending)
        {
            TempData["ErrorMessage"] = "Only pending bookings can be edited";
            return RedirectToAction(nameof(Details), new { id = model.Id });
        }

        // Validate booking date
        if (model.BookingDate.Date < DateTime.Today.AddDays(1).Date)
        {
            ModelState.AddModelError(nameof(model.BookingDate), "Booking date must be at least one day from today");
            await RePopulateRoomSlots(model);
            return View(model);
        }

        // Get current room slots for this booking
        var currentRoomSlots = await _roomSlotService.GetPagedAsync(new GetRoomSlotModel
        {
            BookingId = model.Id,
            PageSize = 100
        });
        // Get room id from current room slots
        var roomId = currentRoomSlots.Data?.Items.FirstOrDefault()?.RoomId;

        if (!currentRoomSlots.IsSuccess || currentRoomSlots.Data == null)
        {
            ModelState.AddModelError(string.Empty, "Failed to retrieve current room slots");
            await RePopulateRoomSlots(model);
            return View(model);
        }

        // Check if all room slots are available for the new date
        var existingBookings = await _bookingService.GetPagedAsync(new GetBookingModel
        {
            BookingDate = model.BookingDate,
            RoomId = roomId,
            PageSize = 1000
        });

        if (existingBookings is { IsSuccess: true, Data: not null })
        {
            var bookedSlots = new HashSet<(Guid RoomId, TimeSlot TimeSlot)>();
            
            foreach (var booking in existingBookings.Data.Items.Where(b => b.Id != model.Id))
            {
                var slots = await _roomSlotService.GetPagedAsync(new GetRoomSlotModel
                {
                    BookingId = booking.Id,
                    PageSize = 100
                });

                if (!slots.IsSuccess || slots.Data == null) continue;

                foreach (var slot in slots.Data.Items)
                {
                    bookedSlots.Add((slot.RoomId, slot.TimeSlot));
                }
            }

            // Check if any of current room slots conflict with existing bookings
            if (currentRoomSlots.Data.Items.Any(slot => bookedSlots.Contains((slot.RoomId, slot.TimeSlot))))
            {
                ModelState.AddModelError(string.Empty, 
                    "One or more selected room slots are not available for the new date");
                await RePopulateRoomSlots(model);
                return View(model);
            }
        }

        // Update booking date
        bookingResult.Data.BookingDate = model.BookingDate;
        var updateResult = await _bookingService.UpdateAsync(bookingResult.Data);

        if (updateResult.IsSuccess)
        {
            TempData["SuccessMessage"] = "Booking date updated successfully";
            return RedirectToAction(nameof(Details), new { id = model.Id });
        }

        ModelState.AddModelError(string.Empty, updateResult.Error ?? "Failed to update booking");
        await RePopulateRoomSlots(model);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(Guid id)
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

        // Delete room slots associated with this booking
        var roomSlos = await _roomSlotService.GetPagedAsync(new GetRoomSlotModel
        {
            BookingId = id,
            PageSize = 100
        });
        if (roomSlos is { IsSuccess: true, Data: not null })
        {
            foreach (var slot in roomSlos.Data.Items)
            {
                await _roomSlotService.DeleteAsync(slot.Id);
            }
        }

        TempData["SuccessMessage"] = "Booking cancelled successfully";
        return RedirectToAction(nameof(Details), new { id });
    }

    private async Task PopulateAvailableRoomSlots(CreateBookingViewModel model)
    {
        // Get all rooms based on filters
        var roomQuery = new GetRoomModel
        {
            DepartmentId = model.DepartmentId,
            CampusId = model.CampusId,
            PageSize = 100
        };

        var roomsResult = await _roomService.GetPagedAsync(roomQuery);
        if (!roomsResult.IsSuccess || roomsResult.Data == null) return;

        // Get existing bookings for the selected date
        var bookingsQuery = new GetBookingModel
        {
            BookingDate = model.BookingDate,
            PageSize = 1000
        };

        var bookingsResult = await _bookingService.GetPagedAsync(bookingsQuery);
        var bookedSlots = new HashSet<(Guid RoomId, TimeSlot TimeSlot)>();

        if (bookingsResult is { IsSuccess: true, Data: not null })
        {
            foreach (var booking in bookingsResult.Data.Items)
            {
                var slotsResult = await _roomSlotService.GetPagedAsync(new GetRoomSlotModel
                {
                    BookingId = booking.Id,
                    PageSize = 100
                });

                if (!slotsResult.IsSuccess || slotsResult.Data == null) continue;

                foreach (var slot in slotsResult.Data.Items)
                {
                    bookedSlots.Add((slot.RoomId, slot.TimeSlot));
                }
            }
        }

        // Generate available slots
        model.AvailableRoomSlots.Clear();
        foreach (var room in roomsResult.Data.Items)
        {
            foreach (TimeSlot timeSlot in Enum.GetValues(typeof(TimeSlot)))
            {
                if (bookedSlots.Contains((room.Id, timeSlot))) continue;

                model.AvailableRoomSlots.Add(new RoomSlotViewModel
                {
                    RoomId = room.Id,
                    RoomName = room.Name,
                    DepartmentName = room.Department.Name,
                    CampusName = room.Department.Campus.Name,
                    TimeSlot = timeSlot,
                });
            }
        }
    }

    private async Task PopulateDropdowns(Guid? campusId = null, Guid? departmentId = null)
    {
        // Get campuses
        var campusesResult = await _campusService.GetPagedAsync(new GetCampusModel
        {
            PageSize = 100 // Get a reasonably large number of campuses
        });
        ViewBag.Campuses = campusesResult.IsSuccess ? campusesResult.Data?.Items : new List<CampusModel>();

        // Get departments for selected campus
        var departmentsResult = await _departmentService.GetPagedAsync(new GetDepartmentModel
        {
            PageSize = 100,
            CampusId = campusId
        });
        ViewBag.Departments = departmentsResult.IsSuccess ? departmentsResult.Data?.Items : new List<DepartmentModel>();

        // Get rooms for selected department
        var roomsResult = await _roomService.GetPagedAsync(new GetRoomModel
        {
            PageSize = 100,
            DepartmentId = departmentId
        });
        ViewBag.Rooms = roomsResult.IsSuccess ? roomsResult.Data?.Items : new List<RoomModel>();
    }

    [HttpGet]
    public async Task<IActionResult> GetDepartments(Guid? campusId)
    {
        if (!campusId.HasValue)
            return Json(new List<object>());

        var departmentsResult = await _departmentService.GetPagedAsync(new GetDepartmentModel
        {
            PageSize = 100,
            CampusId = campusId
        });

        if (!departmentsResult.IsSuccess || departmentsResult.Data == null)
            return Json(new List<object>());

        var departments = departmentsResult.Data.Items.Select(d => new
        {
            id = d.Id,
            name = d.Name
        });

        return Json(departments);
    }

    [HttpGet]
    public async Task<IActionResult> GetRooms(Guid? departmentId)
    {
        if (!departmentId.HasValue)
            return Json(new List<object>());

        var roomsResult = await _roomService.GetPagedAsync(new GetRoomModel
        {
            PageSize = 100,
            DepartmentId = departmentId
        });

        if (!roomsResult.IsSuccess || roomsResult.Data == null)
            return Json(new List<object>());

        var rooms = roomsResult.Data.Items.Select(r => new
        {
            id = r.Id,
            name = r.Name
        });

        return Json(rooms);
    }

    [HttpGet]
    public async Task<IActionResult> GetAvailableRoomSlots(DateTime? bookingDate, Guid? campusId, Guid? departmentId,
        Guid? roomId)
    {
        var model = new CreateBookingViewModel
        {
            BookingDate = bookingDate ?? DateTime.Today,
            CampusId = campusId,
            DepartmentId = departmentId,
            RoomId = roomId
        };

        // Check if all required filters are selected
        if (!bookingDate.HasValue || !campusId.HasValue || !departmentId.HasValue || !roomId.HasValue)
        {
            return PartialView("_AvailableRoomSlotsPartial", model);
        }

        // Get all rooms based on filters
        var roomQuery = new GetRoomModel
        {
            DepartmentId = model.DepartmentId,
            CampusId = model.CampusId,
            PageSize = 100
        };

        var roomsResult = await _roomService.GetPagedAsync(roomQuery);
        if (!roomsResult.IsSuccess || roomsResult.Data == null)
        {
            model.AvailableRoomSlots.Clear();
            return PartialView("_AvailableRoomSlotsPartial", model);
        }

        // Filter rooms if specific room is selected
        var rooms = roomsResult.Data.Items;
        if (model.RoomId.HasValue)
        {
            rooms = rooms.Where(r => r.Id == model.RoomId.Value).ToList();
        }

        // Get existing bookings for the selected date
        var bookingsQuery = new GetBookingModel
        {
            BookingDate = model.BookingDate,
            DepartmentId = model.DepartmentId,
            RoomId = model.RoomId,
            PageSize = 1000
        };

        var bookingsResult = await _bookingService.GetPagedAsync(bookingsQuery);
        var bookedSlots = new HashSet<(Guid RoomId, TimeSlot TimeSlot)>();

        if (bookingsResult is { IsSuccess: true, Data: not null })
        {
            var bookings = bookingsResult.Data.Items
                .Where(b => b.Status != BookingStatus.Cancelled && b.Status != BookingStatus.Rejected).ToList();
            foreach (var booking in bookings)
            {
                var slotsResult = await _roomSlotService.GetPagedAsync(new GetRoomSlotModel
                {
                    BookingId = booking.Id,
                    PageSize = 100
                });

                if (!slotsResult.IsSuccess || slotsResult.Data == null) continue;

                foreach (var slot in slotsResult.Data.Items)
                {
                    bookedSlots.Add((slot.RoomId, slot.TimeSlot));
                }
            }
        }

        // Generate available slots
        model.AvailableRoomSlots.Clear();
        foreach (var room in rooms)
        {
            foreach (TimeSlot timeSlot in Enum.GetValues(typeof(TimeSlot)))
            {
                if (bookedSlots.Contains((room.Id, timeSlot))) continue;

                model.AvailableRoomSlots.Add(new RoomSlotViewModel
                {
                    RoomId = room.Id,
                    RoomName = room.Name,
                    DepartmentName = room.Department.Name,
                    CampusName = room.Department.Campus.Name,
                    TimeSlot = timeSlot,
                });
            }
        }

        return PartialView("_AvailableRoomSlotsPartial", model);
    }

    private async Task RePopulateRoomSlots(EditBookingViewModel model)
    {
        var roomSlotsModel = new GetRoomSlotModel
        {
            BookingId = model.Id,
            PageSize = 100
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
                model.RoomSlots.Add(roomSlotViewModel);
            }
        }
        model.IsManager = false;
    }
}
