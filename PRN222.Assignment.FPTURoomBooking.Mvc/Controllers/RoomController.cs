using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PRN222.Assignment.FPTURoomBooking.Mvc.Models;
using PRN222.Assignment.FPTURoomBooking.Services.Models.Room;
using PRN222.Assignment.FPTURoomBooking.Services.Models.Campus;
using PRN222.Assignment.FPTURoomBooking.Services.Services.Interfaces;
using PRN222.Assignment.FPTURoomBooking.Services.Utils;

namespace PRN222.Assignment.FPTURoomBooking.Mvc.Controllers;

[Authorize(Roles = "User")]
public class RoomController : Controller
{
    private readonly IRoomService _roomService;
    private readonly ICampusService _campusService;

    public RoomController(
        IRoomService roomService,
        ICampusService campusService)
    {
        _roomService = roomService;
        _campusService = campusService;
    }

    public async Task<IActionResult> Index(RoomListViewModel model)
    {
        // Get campuses for dropdown
        var campusesResult = await _campusService.GetPagedAsync(new GetCampusModel
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
            CampusId = model.CampusId
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
            viewModel.CampusName = room.Campus.Name;
            rooms.Add(viewModel);
        }

        var resultModel = new RoomListViewModel
        {
            Rooms = new PaginationResult<RoomViewModel>(
                rooms,
                result.Data.TotalItems,
                result.Data.PageNumber,
                result.Data.PageSize),
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
        var roomResult = await _roomService.GetAsync(id);

        if (!roomResult.IsSuccess || roomResult.Data == null)
        {
            return NotFound(roomResult.Error);
        }

        var room = roomResult.Data.Adapt<RoomViewModel>();
        room.CampusName = roomResult.Data.Campus.Name;

        return View(room);
    }
}