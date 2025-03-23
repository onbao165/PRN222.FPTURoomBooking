using PRN222.Assignment.FPTURoomBooking.Services.Models.Booking;
using PRN222.Assignment.FPTURoomBooking.Services.Models.RoomSlot;
using PRN222.Assignment.FPTURoomBooking.Services.Utils;

namespace PRN222.Assignment.FPTURoomBooking.Services.Services.Interfaces;

public interface IBookingService
{
    Task<Result> CreateAsync(BookingModel model);
    Task<Result> UpdateAsync(BookingModel model);
    Task<Result> DeleteAsync(Guid id);
    Task<Result<BookingModel>> GetAsync(Guid id);
    Task<Result<PaginationResult<BookingModel>>> GetPagedAsync(GetBookingModel model);
    Task<Result<BookingModel>> CreateBookingWithRoomSlots(BookingModel booking, IEnumerable<RoomSlotModel> roomSlots);
}
    