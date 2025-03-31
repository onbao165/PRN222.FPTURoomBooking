using PRN222.Assignment.FPTURoomBooking.Services.Utils;

namespace PRN222.Assignment.FPTURoomBooking.Mvc.Models;

public class CampusViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CampusListViewModel
{
    public PaginationResult<CampusViewModel> Campuses { get; set; } = null!;
    public string? SearchTerm { get; set; }
    public string? OrderBy { get; set; }
    public bool IsDescending { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
} 