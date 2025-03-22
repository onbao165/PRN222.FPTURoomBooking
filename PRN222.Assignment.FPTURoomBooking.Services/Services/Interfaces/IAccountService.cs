using PRN222.Assignment.FPTURoomBooking.Services.Models.Account;
using PRN222.Assignment.FPTURoomBooking.Services.Utils;

namespace PRN222.Assignment.FPTURoomBooking.Services.Services.Interfaces;

public interface IAccountService
{
    Task<Result> CreateAsync(AccountModel model);
    Task<Result> UpdateAsync(AccountModel model);
    Task<Result> DeleteAsync(Guid id);
    Task<Result<AccountModel>> GetAsync(Guid id);
    Task<Result<PaginationResult<AccountModel>>> GetPagedAsync(GetAccountModel model);
    Task<Result<AccountModel>> LoginAsync(string email, string password);
}