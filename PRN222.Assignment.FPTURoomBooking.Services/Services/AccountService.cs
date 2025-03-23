using System.Linq.Expressions;
using Mapster;
using Microsoft.EntityFrameworkCore;
using PRN222.Assignment.FPTURoomBooking.Repositories.Models;
using PRN222.Assignment.FPTURoomBooking.Repositories.UnitOfWork;
using PRN222.Assignment.FPTURoomBooking.Services.Models.Account;
using PRN222.Assignment.FPTURoomBooking.Services.Services.Interfaces;
using PRN222.Assignment.FPTURoomBooking.Services.Utils;
using PRN222.Assignment.FPTURoomBooking.Services.Utils.PasswordHasher;

namespace PRN222.Assignment.FPTURoomBooking.Services.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;

        public AccountService(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }

        public async Task<Result> CreateAsync(AccountModel model)
        {
            var entity = model.Adapt<Account>();
            _unitOfWork.AccountRepository.Add(entity);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            var entity = await _unitOfWork.AccountRepository.GetByIdAsync(id);
            if (entity == null)
            {
                return Result.Failure("Account not found");
            }

            _unitOfWork.AccountRepository.Remove(entity);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result<AccountModel>> GetAsync(Guid id)
        {
            var entity = await _unitOfWork.AccountRepository.GetQueryable()
                .Include(x => x.Department)
                .ProjectToType<AccountModel>()
                .FirstOrDefaultAsync(x => x.Id == id);
            return entity ?? Result<AccountModel>.Failure("Account not found");
        }

        public async Task<Result<PaginationResult<AccountModel>>> GetPagedAsync(GetAccountModel model)
        {
            var query = _unitOfWork.AccountRepository.GetQueryable();
            Expression<Func<Account, bool>> filter = x => true;

            if (!string.IsNullOrEmpty(model.SearchTerm))
            {
                filter = filter.CombineAndAlsoExpressions(x => true);
            }

            if (!model.DepartmentId.IsNullOrGuidEmpty())
            {
                filter = filter.CombineAndAlsoExpressions(x => x.DepartmentId == model.DepartmentId);
            }

            query = query.Where(filter);
            query = query.ApplySorting(model.IsDescending, Account.GetSortValue(model.OrderBy));

            return await query.ProjectToPaginatedListAsync<Account, AccountModel>(model);
        }

        public async Task<Result<AccountModel>> LoginAsync(string email, string password)
        {
            var entity = await _unitOfWork.AccountRepository.GetQueryable().FirstOrDefaultAsync(x => x.Email == email);
            if (entity == null)
            {
                return Result<AccountModel>.Failure("Account not found");
            }

            if (!_passwordHasher.VerifyPassword(password, entity.Password))
            {
                return Result<AccountModel>.Failure("Invalid password");
            }

            return entity.Adapt<AccountModel>();
        }

        public async Task<Result> UpdateAsync(AccountModel model)
        {
            var entity = await _unitOfWork.AccountRepository.GetByIdAsync(model.Id);
            if (entity == null)
            {
                return Result.Failure("Account not found");
            }

            entity = model.Adapt(entity);
            _unitOfWork.AccountRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result<AccountModel>> GetByEmailAsync(string email)
        {
            var entity = await _unitOfWork.AccountRepository.GetQueryable()
                .Include(x => x.Department)
                .ThenInclude(d => d.Campus)
                .FirstOrDefaultAsync(x => x.Email == email);

            if (entity == null)
            {
                return Result<AccountModel>.Failure("Account not found");
            }

            return entity.Adapt<AccountModel>();
        }
    }
}