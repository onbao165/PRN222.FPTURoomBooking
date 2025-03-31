using PRN222.Assignment.FPTURoomBooking.Services.Utils;

namespace PRN222.Assignment.FPTURoomBooking.Mvc.Models;

public class DepartmentViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid CampusId { get; set; }
    public string CampusName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class DepartmentListViewModel
{
    public PaginationResult<DepartmentViewModel> Departments { get; set; } = null!;
    public string? SearchTerm { get; set; }
    public string? OrderBy { get; set; }
    public bool IsDescending { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public Guid? CampusId { get; set; }
} 