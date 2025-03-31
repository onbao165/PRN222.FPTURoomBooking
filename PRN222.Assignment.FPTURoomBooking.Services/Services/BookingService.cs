using System.Linq.Expressions;
using Mapster;
using Microsoft.EntityFrameworkCore;
using PRN222.Assignment.FPTURoomBooking.Repositories.Models;
using PRN222.Assignment.FPTURoomBooking.Repositories.UnitOfWork;
using PRN222.Assignment.FPTURoomBooking.Services.Models.Booking;
using PRN222.Assignment.FPTURoomBooking.Services.Models.RoomSlot;
using PRN222.Assignment.FPTURoomBooking.Services.Models.Slot;
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
            var entity = await _unitOfWork.BookingRepository.GetQueryable()
                .Include(x => x.Account)
                .ProjectToType<BookingModel>()
                .FirstOrDefaultAsync(x => x.Id == id);
            return entity ?? Result<BookingModel>.Failure("Booking not found");
        }

        public async Task<Result<PaginationResult<BookingModel>>> GetPagedAsync(GetBookingModel model)
        {
            try
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

                if (!model.RoomId.IsNullOrGuidEmpty())
                {
                    filter = filter.CombineAndAlsoExpressions(x => x.RoomSlots.Any(rs => rs.RoomId == model.RoomId));
                }

                if (!model.ManagerId.IsNullOrGuidEmpty())
                {
                    filter = filter.CombineAndAlsoExpressions(x => x.ManagerId == model.ManagerId);
                }

                if (model.BookingDate.HasValue)
                {
                    filter = filter.CombineAndAlsoExpressions(x => x.BookingDate == model.BookingDate.Value);
                }

                if (model.StartDate.HasValue)
                {
                    filter = filter.CombineAndAlsoExpressions(x => x.BookingDate >= model.StartDate.Value.Date);
                }

                if (model.EndDate.HasValue)
                {
                    filter = filter.CombineAndAlsoExpressions(x => x.BookingDate <= model.EndDate.Value.Date);
                }

                if (model.Status.HasValue)
                {
                    filter = filter.CombineAndAlsoExpressions(x => x.Status == model.Status);
                }

                query = query.Where(filter);
                query = query.ApplySorting(model.IsDescending, Booking.GetSortValue(model.OrderBy));

                return await query.ProjectToPaginatedListAsync<Booking, BookingModel>(model);
            }
            catch (Exception ex)
            {
                return Result<PaginationResult<BookingModel>>.Failure($"Failed to get bookings: {ex.Message}");
            }
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

        public async Task<Result<BookingModel>> CreateBookingWithRoomSlots(BookingModel booking,
            IEnumerable<RoomSlotModel> roomSlots)
        {
            try
            {
                // Create booking
                var bookingEntity = booking.Adapt<Booking>();
                _unitOfWork.BookingRepository.Add(bookingEntity);

                // Create room slots
                foreach (var slot in roomSlots)
                {
                    var roomSlotEntity = slot.Adapt<RoomSlot>();
                    _unitOfWork.RoomSlotRepository.Add(roomSlotEntity);
                }

                await _unitOfWork.SaveChangesAsync();

                // Return the created booking
                var createdBooking = bookingEntity.Adapt<BookingModel>();
                return createdBooking;
            }
            catch (Exception ex)
            {
                return Result<BookingModel>.Failure($"Failed to create booking: {ex.Message}");
            }
        }

        public async Task<Result<BookingModel>> CreateBookingWithSlots(BookingModel booking, SlotModel slot)
        {
            try
            {
                // Create booking
                var bookingEntity = booking.Adapt<Booking>();
                _unitOfWork.BookingRepository.Add(bookingEntity);

                // Create slot
                var slotEntity = slot.Adapt<Slot>();
                _unitOfWork.SlotRepository.Add(slotEntity);

                await _unitOfWork.SaveChangesAsync();

                // Return the created booking
                var createdBooking = bookingEntity.Adapt<BookingModel>();
                return createdBooking;
            }
            catch (Exception ex)
            {
                return Result<BookingModel>.Failure($"Failed to create booking: {ex.Message}");
            }
        }

        public async Task<Result> UpdateStatusAsync(Guid id, Guid managerId, BookingStatus status)
        {
            var entity = await _unitOfWork.BookingRepository.GetQueryable()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                return Result.Failure("Booking not found");
            }

            // Only allow updating status if current status is Pending
            if (entity.Status != BookingStatus.Pending)
            {
                return Result.Failure("Can only update status of pending bookings");
            }

            entity.Status = status;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.ManagerId = managerId;

            _unitOfWork.BookingRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }
    }
}