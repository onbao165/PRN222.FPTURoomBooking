using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PRN222.Assignment.FPTURoomBooking.Mvc.Models;
using PRN222.Assignment.FPTURoomBooking.Services.Models.Department;
using PRN222.Assignment.FPTURoomBooking.Services.Models.Room;
using PRN222.Assignment.FPTURoomBooking.Services.Services.Interfaces;
using PRN222.Assignment.FPTURoomBooking.Services.Utils;

namespace PRN222.Assignment.FPTURoomBooking.Mvc.Controllers;

[Authorize]
public class DepartmentController : Controller
{
    private readonly IDepartmentService _departmentService;
    private readonly ICampusService _campusService;
    private readonly IRoomService _roomService;

    public DepartmentController(
        IDepartmentService departmentService,
        ICampusService campusService,
        IRoomService roomService)
    {
        _departmentService = departmentService;
        _campusService = campusService;
        _roomService = roomService;
    }

    public async Task<IActionResult> Index(DepartmentListViewModel model)
    {
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

        var departmentModel = new GetDepartmentModel
        {
            PageNumber = model.PageNumber,
            PageSize = model.PageSize,
            SearchTerm = model.SearchTerm ?? string.Empty,
            OrderBy = model.OrderBy ?? "name",
            IsDescending = model.IsDescending,
            CampusId = model.CampusId
        };

        var result = await _departmentService.GetPagedAsync(departmentModel);

        if (!result.IsSuccess || result.Data == null)
        {
            return View("Error", new ErrorViewModel
            {
                Message = result.Error,
                RequestId = HttpContext.TraceIdentifier
            });
        }

        // Map data to view model
        var departments = new List<DepartmentViewModel>();
        foreach (var dept in result.Data.Items)
        {
            var viewModel = dept.Adapt<DepartmentViewModel>();
            // Map campus name from Campus property if available
            viewModel.CampusName = dept.Campus.Name;

            departments.Add(viewModel);
        }

        var resultModel = new DepartmentListViewModel
        {
            Departments = new PaginationResult<DepartmentViewModel>(departments, result.Data.TotalItems, result.Data.PageNumber, result.Data.PageSize),
            SearchTerm = model.SearchTerm,
            OrderBy = model.OrderBy,
            IsDescending = model.IsDescending,
            PageNumber = model.PageNumber,
            PageSize = model.PageSize,
            CampusId = model.CampusId
        };

        return View(resultModel);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var departmentResult = await _departmentService.GetAsync(id);

        if (!departmentResult.IsSuccess || departmentResult.Data == null)
        {
            return NotFound(departmentResult.Error);
        }

        var department = departmentResult.Data.Adapt<DepartmentViewModel>();

        // Ensure we have the campus name
        department.CampusName = departmentResult.Data.Campus.Name;

        // Get rooms for this department
        var roomsModel = new GetRoomModel
        {
            DepartmentId = id,
            PageNumber = 1,
            PageSize = 50, // Show a reasonable number of rooms
            OrderBy = "name"
        };

        var roomsResult = await _roomService.GetPagedAsync(roomsModel);

        ViewBag.Rooms = roomsResult.IsSuccess
            ? roomsResult.Data
            : null;

        return View(department);
    }
}