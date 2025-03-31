using System.Linq.Expressions;
using Mapster;
using Microsoft.EntityFrameworkCore;
using PRN222.Assignment.FPTURoomBooking.Repositories.Models;
using PRN222.Assignment.FPTURoomBooking.Repositories.UnitOfWork;
using PRN222.Assignment.FPTURoomBooking.Services.Models.Slot;
using PRN222.Assignment.FPTURoomBooking.Services.Services.Interfaces;
using PRN222.Assignment.FPTURoomBooking.Services.Utils;

namespace PRN222.Assignment.FPTURoomBooking.Services.Services;

public class SlotService : ISlotService
{
    private readonly IUnitOfWork _unitOfWork;

        public SlotService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> CreateAsync(SlotModel model)
        {
            var entity = model.Adapt<Slot>();
            _unitOfWork.SlotRepository.Add(entity);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            var entity = await _unitOfWork.SlotRepository.GetByIdAsync(id);
            if (entity == null)
            {
                return Result.Failure("Slot not found");
            }
            _unitOfWork.SlotRepository.Remove(entity);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result<SlotModel>> GetAsync(Guid id)
        {
            var entity = await _unitOfWork.SlotRepository.GetQueryable()
                .Include(x => x.Room)
                .Include(x => x.Booking)
                .ProjectToType<SlotModel>()
                .FirstOrDefaultAsync(x => x.Id == id);
            return entity ?? Result<SlotModel>.Failure("Slot not found");
        }

        public async Task<Result<PaginationResult<SlotModel>>> GetPagedAsync(GetSlotModel model)
        {
            var query = _unitOfWork.SlotRepository.GetQueryable();
            Expression<Func<Slot, bool>> filter = x => true;

            if (!string.IsNullOrEmpty(model.SearchTerm))
            {
                filter = filter.CombineAndAlsoExpressions(x => true);
            }
            
            if (!model.RoomId.IsNullOrGuidEmpty())
            {
                filter = filter.CombineAndAlsoExpressions(x => x.RoomId == model.RoomId);
            }
            
            if (!model.BookingId.IsNullOrGuidEmpty())
            {
                filter = filter.CombineAndAlsoExpressions(x => x.BookingId == model.BookingId);
            }
            
            if (model.IncludeDeleted)
            {
                query = query.IgnoreQueryFilters();
            }

            query = query.Where(filter);
            query = query.ApplySorting(model.IsDescending, Slot.GetSortValue(model.OrderBy));

            return await query.ProjectToPaginatedListAsync<Slot, SlotModel>(model);
        }

        public async Task<Result> UpdateAsync(SlotModel model)
        {
            var entity = await _unitOfWork.SlotRepository.GetByIdAsync(model.Id);
            if (entity == null)
            {
                return Result.Failure("Slot not found");
            }

            entity = model.Adapt(entity);
            _unitOfWork.SlotRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
}