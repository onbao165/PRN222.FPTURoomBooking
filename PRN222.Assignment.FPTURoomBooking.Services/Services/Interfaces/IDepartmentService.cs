using PRN222.Assignment.FPTURoomBooking.Services.Models.Department;
using PRN222.Assignment.FPTURoomBooking.Services.Utils;

namespace PRN222.Assignment.FPTURoomBooking.Services.Services.Interfaces;

public interface IDepartmentService
{
    Task<Result> CreateAsync(DepartmentModel model);
    Task<Result> UpdateAsync(DepartmentModel model);
    Task<Result> DeleteAsync(Guid id);
    Task<Result<DepartmentModel>> GetAsync(Guid id);
    Task<Result<PaginationResult<DepartmentModel>>> GetPagedAsync(GetDepartmentModel model);
}
