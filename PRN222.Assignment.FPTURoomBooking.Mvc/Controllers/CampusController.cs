using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN222.Assignment.FPTURoomBooking.Mvc.Models;
using PRN222.Assignment.FPTURoomBooking.Services.Models.Campus;
using PRN222.Assignment.FPTURoomBooking.Services.Models.Department;
using PRN222.Assignment.FPTURoomBooking.Services.Services.Interfaces;
using PRN222.Assignment.FPTURoomBooking.Services.Utils;

namespace PRN222.Assignment.FPTURoomBooking.Mvc.Controllers;

[Authorize]
public class CampusController : Controller
{
    private readonly ICampusService _campusService;
    private readonly IDepartmentService _departmentService;

    public CampusController(ICampusService campusService, IDepartmentService departmentService)
    {
        _campusService = campusService;
        _departmentService = departmentService;
    }

    public async Task<IActionResult> Index(CampusListViewModel model)
    {
        var campusModel = new GetCampusModel
        {
            PageNumber = model.PageNumber,
            PageSize = model.PageSize,
            SearchTerm = model.SearchTerm ?? string.Empty,
            OrderBy = model.OrderBy ?? "name",
            IsDescending = model.IsDescending
        };

        var result = await _campusService.GetPagedAsync(campusModel);

        if (!result.IsSuccess || result.Data == null)
        {
            return View("Error", new ErrorViewModel
            {
                Message = result.Error,
                RequestId = HttpContext.TraceIdentifier
            });
        }

        var resultModel = new CampusListViewModel
        {
            Campuses = new PaginationResult<CampusViewModel>(result.Data.Items.Adapt<List<CampusViewModel>>(), result.Data.TotalItems, result.Data.PageNumber, result.Data.PageSize),
            SearchTerm = model.SearchTerm,
            OrderBy = model.OrderBy,
            IsDescending = model.IsDescending,
            PageNumber = model.PageNumber,
            PageSize = model.PageSize
        };

        return View(resultModel);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var campusResult = await _campusService.GetAsync(id);

        if (!campusResult.IsSuccess)
        {
            return NotFound(campusResult.Error);
        }

        var campus = campusResult.Data.Adapt<CampusViewModel>();

        var departmentsModel = new GetDepartmentModel
        {
            CampusId = id,
            PageNumber = 1,
            PageSize = 50, // Show a reasonable number of departments
            OrderBy = "name"
        };

        var departmentsResult = await _departmentService.GetPagedAsync(departmentsModel);

        ViewBag.Departments = departmentsResult.IsSuccess
            ? departmentsResult.Data
            : null;

        return View(campus);
    }
}