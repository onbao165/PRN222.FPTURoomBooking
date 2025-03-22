using System.Linq.Expressions;
using Mapster;
using PRN222.Assignment.FPTURoomBooking.Repositories.Models;
using PRN222.Assignment.FPTURoomBooking.Repositories.UnitOfWork;
using PRN222.Assignment.FPTURoomBooking.Services.Models.Department;
using PRN222.Assignment.FPTURoomBooking.Services.Services.Interfaces;
using PRN222.Assignment.FPTURoomBooking.Services.Utils;

namespace PRN222.Assignment.FPTURoomBooking.Services.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> CreateAsync(DepartmentModel model)
        {
            var entity = model.Adapt<Department>();
            _unitOfWork.DepartmentRepository.Add(entity);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            var entity = await _unitOfWork.DepartmentRepository.GetByIdAsync(id);
            if (entity == null)
            {
                return Result.Failure("Department not found");
            }
            _unitOfWork.DepartmentRepository.Remove(entity);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result<DepartmentModel>> GetAsync(Guid id)
        {
            var entity = await _unitOfWork.DepartmentRepository.GetByIdAsync(id);
            if (entity == null)
            {
                return Result<DepartmentModel>.Failure("Department not found");
            }
            return entity.Adapt<DepartmentModel>();
        }

        public async Task<Result<PaginationResult<DepartmentModel>>> GetPagedAsync(GetDepartmentModel model)
        {
            var query = _unitOfWork.DepartmentRepository.GetQueryable();
            Expression<Func<Department, bool>> filter = x => true;

            if (!string.IsNullOrEmpty(model.SearchTerm))
            {
                filter = filter.CombineAndAlsoExpressions(x => true);
            }
            
            if (!model.CampusId.IsNullOrGuidEmpty())
            {
                filter = filter.CombineAndAlsoExpressions(x => x.CampusId == model.CampusId);
            }

            query = query.Where(filter);
            query = query.ApplySorting(model.IsDescending, Department.GetSortValue(model.OrderBy));

            return await query.ProjectToPaginatedListAsync<Department, DepartmentModel>(model);
        }

        public async Task<Result> UpdateAsync(DepartmentModel model)
        {
            var entity = await _unitOfWork.DepartmentRepository.GetByIdAsync(model.Id);
            if (entity == null)
            {
                return Result.Failure("Department not found");
            }

            entity = model.Adapt(entity);
            _unitOfWork.DepartmentRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
    }
}