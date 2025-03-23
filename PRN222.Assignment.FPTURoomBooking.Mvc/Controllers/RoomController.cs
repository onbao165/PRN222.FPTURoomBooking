using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PRN222.Assignment.FPTURoomBooking.Mvc.Models;
using PRN222.Assignment.FPTURoomBooking.Services.Models.Room;
using PRN222.Assignment.FPTURoomBooking.Services.Services.Interfaces;
using PRN222.Assignment.FPTURoomBooking.Services.Utils;

namespace PRN222.Assignment.FPTURoomBooking.Mvc.Controllers;

[Authorize(Roles = "User")]
public class RoomController : Controller
{
     private readonly IRoomService _roomService;
    private readonly IDepartmentService _departmentService;
    private readonly ICampusService _campusService;

    public RoomController(
        IRoomService roomService,
        IDepartmentService departmentService,
        ICampusService campusService)
    {
        _roomService = roomService;
        _departmentService = departmentService;
        _campusService = campusService;
    }

    public async Task<IActionResult> Index(RoomListViewModel model)
    {
        // Get departments for dropdown
        var departmentsResult = await _departmentService.GetPagedAsync(new Services.Models.Department.GetDepartmentModel
        {
            PageSize = 100, // Get a reasonably large number of departments
            CampusId = model.CampusId
        });

        if (departmentsResult is { IsSuccess: true, Data: not null })
        {
            ViewBag.Departments = new SelectList(
                departmentsResult.Data.Items,
                "Id",
                "Name",
                model.DepartmentId);
        }

        // Get campuses for dropdown
        var campusesResult = await _campusService.GetPagedAsync(new Services.Models.Campus.GetCampusModel
        {
            PageSize = 100 // Get a reasonably large number of campuses
        });

        if (campusesResult is { IsSuccess: true, Data: not null })
        {
            ViewBag.Campuses = new SelectList(
                campusesResult.Data.Items,
                "Id",
                "Name",
                model.CampusId);
        }

        var roomModel = new GetRoomModel
        {
            PageNumber = model.PageNumber,
            PageSize = model.PageSize,
            SearchTerm = model.SearchTerm ?? string.Empty,
            OrderBy = model.OrderBy ?? "name",
            IsDescending = model.IsDescending,
            DepartmentId = model.DepartmentId
        };

        var result = await _roomService.GetPagedAsync(roomModel);

        if (!result.IsSuccess || result.Data == null)
        {
            return View("Error", new ErrorViewModel
            {
                Message = result.Error,
                RequestId = HttpContext.TraceIdentifier
            });
        }

        // Map data to view model
        var rooms = new List<RoomViewModel>();
        foreach (var room in result.Data.Items)
        {
            var viewModel = room.Adapt<RoomViewModel>();
            // Map department name from Department property if available
            viewModel.DepartmentName = room.Department.Name;

            // Map campus name if available
            viewModel.CampusName = room.Department.Campus.Name;

            rooms.Add(viewModel);
        }

        var resultModel = new RoomListViewModel
        {
            Rooms = new PaginationResult<RoomViewModel>(rooms, result.Data.TotalItems, result.Data.PageNumber,
                result.Data.PageSize),
            SearchTerm = model.SearchTerm,
            OrderBy = model.OrderBy,
            IsDescending = model.IsDescending,
            PageNumber = model.PageNumber,
            PageSize = model.PageSize,
            DepartmentId = model.DepartmentId,
            CampusId = model.CampusId
        };

        return View(resultModel);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var roomResult = await _roomService.GetAsync(id);

        if (!roomResult.IsSuccess || roomResult.Data == null)
        {
            return NotFound(roomResult.Error);
        }

        var room = roomResult.Data.Adapt<RoomViewModel>();

        // Ensure we have the department and campus name
        room.DepartmentName = roomResult.Data.Department.Name;
        room.CampusName = roomResult.Data.Department.Campus.Name;

        return View(room);
    }
}