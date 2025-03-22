using PRN222.Assignment.FPTURoomBooking.Services.Utils;

namespace PRN222.Assignment.FPTURoomBooking.Services.Models.Department;

public class GetDepartmentModel : PaginationParams
{
    public Guid? CampusId { get; set; }
}