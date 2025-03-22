using PRN222.Assignment.FPTURoomBooking.Services.Models.RoomSlot;
using PRN222.Assignment.FPTURoomBooking.Services.Utils;

namespace PRN222.Assignment.FPTURoomBooking.Services.Services.Interfaces;

public interface IRoomSlotService
{
    Task<Result> CreateAsync(RoomSlotModel model);
    Task<Result> UpdateAsync(RoomSlotModel model);
    Task<Result> DeleteAsync(Guid id);
    Task<Result<RoomSlotModel>> GetAsync(Guid id);
    Task<Result<PaginationResult<RoomSlotModel>>> GetPagedAsync(GetRoomSlotModel model);
}
