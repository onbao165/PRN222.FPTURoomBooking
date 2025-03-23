using System.Linq.Expressions;
using Mapster;
using Microsoft.EntityFrameworkCore;
using PRN222.Assignment.FPTURoomBooking.Repositories.Models;
using PRN222.Assignment.FPTURoomBooking.Repositories.UnitOfWork;
using PRN222.Assignment.FPTURoomBooking.Services.Models.Booking;
using PRN222.Assignment.FPTURoomBooking.Services.Services.Interfaces;
using PRN222.Assignment.FPTURoomBooking.Services.Utils;

namespace PRN222.Assignment.FPTURoomBooking.Services.Services
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> CreateAsync(BookingModel model)
        {
            var entity = model.Adapt<Booking>();
            _unitOfWork.BookingRepository.Add(entity);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            var entity = await _unitOfWork.BookingRepository.GetByIdAsync(id);
            if (entity == null)
            {
                return Result.Failure("Booking not found");
            }
            _unitOfWork.BookingRepository.Remove(entity);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result<BookingModel>> GetAsync(Guid id)
        {
            var entity = await _unitOfWork.BookingRepository.GetByIdAsync(id);
            if (entity == null)
            {
                return Result<BookingModel>.Failure("Booking not found");
            }
            return entity.Adapt<BookingModel>();
        }

        public async Task<Result<PaginationResult<BookingModel>>> GetPagedAsync(GetBookingModel model)
        {
            var query = _unitOfWork.BookingRepository.GetQueryable();
            Expression<Func<Booking, bool>> filter = x => true;

            if (!string.IsNullOrEmpty(model.SearchTerm))
            {
                filter = filter.CombineAndAlsoExpressions(x => true);
            }
            
            if (!model.AccountId.IsNullOrGuidEmpty())
            {
                filter = filter.CombineAndAlsoExpressions(x => x.AccountId == model.AccountId);
            }
            
            if (!model.ManagerId.IsNullOrGuidEmpty())
            {
                filter = filter.CombineAndAlsoExpressions(x => x.ManagerId == model.ManagerId);
            }
            
            if (!model.DepartmentId.IsNullOrGuidEmpty())
            {
                query = query.Include(x => x.RoomSlots).ThenInclude(x => x.Room);
                filter = filter.CombineAndAlsoExpressions(x => x.RoomSlots.Any(rs => rs.Room.DepartmentId == model.DepartmentId));
            }
            
            if (model.Status.HasValue)
            {
                filter = filter.CombineAndAlsoExpressions(x => x.Status == model.Status);
            }

            query = query.Where(filter);
            query = query.ApplySorting(model.IsDescending, Booking.GetSortValue(model.OrderBy));

            return await query.ProjectToPaginatedListAsync<Booking, BookingModel>(model);
        }

        public async Task<Result> UpdateAsync(BookingModel model)
        {
            var entity = await _unitOfWork.BookingRepository.GetByIdAsync(model.Id);
            if (entity == null)
            {
                return Result.Failure("Booking not found");
            }

            entity = model.Adapt(entity);
            _unitOfWork.BookingRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
    }
}