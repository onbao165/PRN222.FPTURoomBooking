using PRN222.Assignment.FPTURoomBooking.Services.Models.Campus;
using PRN222.Assignment.FPTURoomBooking.Services.Utils;

namespace PRN222.Assignment.FPTURoomBooking.Services.Services.Interfaces;

public interface ICampusService
{
    Task<Result> CreateAsync(CampusModel model);
    Task<Result> UpdateAsync(CampusModel model);
    Task<Result> DeleteAsync(Guid id);
    Task<Result<CampusModel>> GetAsync(Guid id);
    Task<Result<PaginationResult<CampusModel>>> GetPagedAsync(GetCampusModel model);
}
