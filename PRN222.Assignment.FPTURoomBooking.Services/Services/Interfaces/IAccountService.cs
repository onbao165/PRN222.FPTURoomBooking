using PRN222.Assignment.FPTURoomBooking.Repositories.Models;
using PRN222.Assignment.FPTURoomBooking.Services.Models.Account;
using PRN222.Assignment.FPTURoomBooking.Services.Utils;

namespace PRN222.Assignment.FPTURoomBooking.Services.Services.Interfaces;

public interface IAccountService
{
    Task<Result> CreateAsync(InitAccountModel model);
    Task<Result> UpdateAsync(Guid id, InitAccountModel model);
    Task<Result> DeleteAsync(Guid id);
    Task<Result<AccountModel>> GetAsync(Guid id);
    Task<Result<PaginationResult<AccountModel>>> GetPagedAsync(GetAccountModel model);
    Task<Result<AccountModel>> LoginAsync(string email, string password);
    Task<Result<AccountModel>> GetByEmailAsync(string email);
    Task<Result<List<Department>>> GetDepartmentsAsync();
    Task<Result<List<AccountModel>>> GetManagersAsync(Guid departmentId);
}