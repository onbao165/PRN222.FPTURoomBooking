using System.Linq.Expressions;
using Mapster;
using PRN222.Assignment.FPTURoomBooking.Repositories.Models;
using PRN222.Assignment.FPTURoomBooking.Repositories.UnitOfWork;
using PRN222.Assignment.FPTURoomBooking.Services.Models.RoomSlot;
using PRN222.Assignment.FPTURoomBooking.Services.Services.Interfaces;
using PRN222.Assignment.FPTURoomBooking.Services.Utils;

namespace PRN222.Assignment.FPTURoomBooking.Services.Services
{
    public class RoomSlotService : IRoomSlotService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoomSlotService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> CreateAsync(RoomSlotModel model)
        {
            var entity = model.Adapt<RoomSlot>();
            _unitOfWork.RoomSlotRepository.Add(entity);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            var entity = await _unitOfWork.RoomSlotRepository.GetByIdAsync(id);
            if (entity == null)
            {
                return Result.Failure("RoomSlot not found");
            }
            _unitOfWork.RoomSlotRepository.Remove(entity);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result<RoomSlotModel>> GetAsync(Guid id)
        {
            var entity = await _unitOfWork.RoomSlotRepository.GetByIdAsync(id);
            if (entity == null)
            {
                return Result<RoomSlotModel>.Failure("RoomSlot not found");
            }
            return entity.Adapt<RoomSlotModel>();
        }

        public async Task<Result<PaginationResult<RoomSlotModel>>> GetPagedAsync(GetRoomSlotModel model)
        {
            var query = _unitOfWork.RoomSlotRepository.GetQueryable();
            Expression<Func<RoomSlot, bool>> filter = x => true;

            if (!string.IsNullOrEmpty(model.SearchTerm))
            {
                filter = filter.CombineAndAlsoExpressions(x => true);
            }

            query = query.Where(filter);
            query = query.ApplySorting(model.IsDescending, RoomSlot.GetSortValue(model.OrderBy));

            return await query.ProjectToPaginatedListAsync<RoomSlot, RoomSlotModel>(model);
        }

        public async Task<Result> UpdateAsync(RoomSlotModel model)
        {
            var entity = await _unitOfWork.RoomSlotRepository.GetByIdAsync(model.Id);
            if (entity == null)
            {
                return Result.Failure("RoomSlot not found");
            }

            entity = model.Adapt(entity);
            _unitOfWork.RoomSlotRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
    }
}