using PRN222.Assignment.FPTURoomBooking.Services.Models.Slot;
using PRN222.Assignment.FPTURoomBooking.Services.Utils;

namespace PRN222.Assignment.FPTURoomBooking.Services.Services.Interfaces;

public interface ISlotService
{
    Task<Result> CreateAsync(SlotModel model);
    Task<Result> UpdateAsync(SlotModel model);
    Task<Result> DeleteAsync(Guid id);
    Task<Result<SlotModel>> GetAsync(Guid id);
    Task<Result<SlotModel>> GetByBookingIdAsync(Guid bookingId);
    Task<Result<PaginationResult<SlotModel>>> GetPagedAsync(GetSlotModel model);
    Task<Result<IEnumerable<SlotModel>>> GetByRoomAndDateAsync(Guid roomId, DateTime date);
}
