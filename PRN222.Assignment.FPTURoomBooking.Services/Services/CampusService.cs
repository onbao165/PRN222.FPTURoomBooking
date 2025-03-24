using System.Linq.Expressions;
using Mapster;
using Microsoft.EntityFrameworkCore;
using PRN222.Assignment.FPTURoomBooking.Repositories.Models;
using PRN222.Assignment.FPTURoomBooking.Repositories.UnitOfWork;
using PRN222.Assignment.FPTURoomBooking.Services.Models.Campus;
using PRN222.Assignment.FPTURoomBooking.Services.Services.Interfaces;
using PRN222.Assignment.FPTURoomBooking.Services.Utils;

namespace PRN222.Assignment.FPTURoomBooking.Services.Services
{
    public class CampusService : ICampusService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CampusService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> CreateAsync(CampusModel model)
        {
            var entity = model.Adapt<Campus>();
            _unitOfWork.CampusRepository.Add(entity);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            var entity = await _unitOfWork.CampusRepository.GetQueryable()
                .Include(x => x.Departments)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                return Result.Failure("Campus not found");
            }

            if (entity.Departments.Any())
            {
                return Result.Failure("Cannot delete campus with associated departments");
            }

            _unitOfWork.CampusRepository.Remove(entity);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result<CampusModel>> GetAsync(Guid id)
        {
            var entity = await _unitOfWork.CampusRepository.GetByIdAsync(id);
            if (entity == null)
            {
                return Result<CampusModel>.Failure("Campus not found");
            }

            return entity.Adapt<CampusModel>();
        }

        public async Task<Result<PaginationResult<CampusModel>>> GetPagedAsync(GetCampusModel model)
        {
            var query = _unitOfWork.CampusRepository.GetQueryable();
            Expression<Func<Campus, bool>> filter = x => true;

            if (!string.IsNullOrEmpty(model.SearchTerm))
            {
                filter = filter.CombineAndAlsoExpressions(x => x.Name.Contains(model.SearchTerm));
            }

            query = query.Where(filter);
            query = query.ApplySorting(model.IsDescending, Campus.GetSortValue(model.OrderBy));

            return await query.ProjectToPaginatedListAsync<Campus, CampusModel>(model);
        }

        public async Task<Result> UpdateAsync(CampusModel model)
        {
            var entity = await _unitOfWork.CampusRepository.GetByIdAsync(model.Id);
            if (entity == null)
            {
                return Result.Failure("Campus not found");
            }

            entity = model.Adapt(entity);
            _unitOfWork.CampusRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
    }
}