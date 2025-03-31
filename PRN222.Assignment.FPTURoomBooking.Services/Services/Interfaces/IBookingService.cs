using PRN222.Assignment.FPTURoomBooking.Services.Models.Booking;
using PRN222.Assignment.FPTURoomBooking.Repositories.Models;
using PRN222.Assignment.FPTURoomBooking.Services.Models.Slot;
using PRN222.Assignment.FPTURoomBooking.Services.Utils;

namespace PRN222.Assignment.FPTURoomBooking.Services.Services.Interfaces;

public interface IBookingService
{
    Task<Result> CreateAsync(BookingModel model);
    Task<Result> UpdateAsync(BookingModel model);
    Task<Result> DeleteAsync(Guid id);
    Task<Result<BookingModel>> GetAsync(Guid id);
    Task<Result<PaginationResult<BookingModel>>> GetPagedAsync(GetBookingModel model);
    Task<Result<BookingModel>> CreateBookingWithSlots(BookingModel booking, SlotModel slot);
    Task<Result> UpdateStatusAsync(Guid id,Guid managerId, BookingStatus status);
}
