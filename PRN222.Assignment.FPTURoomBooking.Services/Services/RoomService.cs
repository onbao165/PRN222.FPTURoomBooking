using System.Linq.Expressions;
using Mapster;
using Microsoft.EntityFrameworkCore;
using PRN222.Assignment.FPTURoomBooking.Repositories.Models;
using PRN222.Assignment.FPTURoomBooking.Repositories.UnitOfWork;
using PRN222.Assignment.FPTURoomBooking.Services.Models.Room;
using PRN222.Assignment.FPTURoomBooking.Services.Services.Interfaces;
using PRN222.Assignment.FPTURoomBooking.Services.Utils;

namespace PRN222.Assignment.FPTURoomBooking.Services.Services
{
    public class RoomService : IRoomService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoomService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> CreateAsync(RoomModel model)
        {
            var entity = model.Adapt<Room>();
            _unitOfWork.RoomRepository.Add(entity);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            var entity = await _unitOfWork.RoomRepository.GetByIdAsync(id);
            if (entity == null)
            {
                return Result.Failure("Room not found");
            }
            _unitOfWork.RoomRepository.Remove(entity);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result<RoomModel>> GetAsync(Guid id)
        {
            var entity = await _unitOfWork.RoomRepository.GetQueryable()
                .Include(x => x.Department)
                .ProjectToType<RoomModel>()
                .FirstOrDefaultAsync(x => x.Id == id);
            return entity ?? Result<RoomModel>.Failure("Room not found");
        }

        public async Task<Result<PaginationResult<RoomModel>>> GetPagedAsync(GetRoomModel model)
        {
            var query = _unitOfWork.RoomRepository.GetQueryable();
            Expression<Func<Room, bool>> filter = x => true;

            if (!string.IsNullOrEmpty(model.SearchTerm))
            {
                filter = filter.CombineAndAlsoExpressions(x => true);
            }
            
            if (!model.DepartmentId.IsNullOrGuidEmpty())
            {
                filter = filter.CombineAndAlsoExpressions(x => x.DepartmentId == model.DepartmentId);
            }
            
            if (!model.CampusId.IsNullOrGuidEmpty())
            {
                filter = filter.CombineAndAlsoExpressions(x => x.Department.CampusId == model.CampusId);
            }

            query = query.Where(filter);
            query = query.ApplySorting(model.IsDescending, Room.GetSortValue(model.OrderBy));

            return await query.ProjectToPaginatedListAsync<Room, RoomModel>(model);
        }

        public async Task<Result> UpdateAsync(RoomModel model)
        {
            var entity = await _unitOfWork.RoomRepository.GetByIdAsync(model.Id);
            if (entity == null)
            {
                return Result.Failure("Room not found");
            }

            entity = model.Adapt(entity);
            _unitOfWork.RoomRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
    }
}