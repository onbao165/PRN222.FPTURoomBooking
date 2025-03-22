using PRN222.Assignment.FPTURoomBooking.Services.Models.Room;
using PRN222.Assignment.FPTURoomBooking.Services.Utils;

namespace PRN222.Assignment.FPTURoomBooking.Services.Services.Interfaces;

public interface IRoomService
{
    Task<Result> CreateAsync(RoomModel model);
    Task<Result> UpdateAsync(RoomModel model);
    Task<Result> DeleteAsync(Guid id);
    Task<Result<RoomModel>> GetAsync(Guid id);
    Task<Result<PaginationResult<RoomModel>>> GetPagedAsync(GetRoomModel model);
}
