using PRN222.Assignment.FPTURoomBooking.Repositories.Models;
using PRN222.Assignment.FPTURoomBooking.Repositories.Repositories;

namespace PRN222.Assignment.FPTURoomBooking.Repositories.UnitOfWork;

/// <summary>
/// Unit of Work interface to manage transactions and repositories
/// </summary>
public interface IUnitOfWork : IDisposable
{
    IGenericRepository<Account, Guid> AccountRepository { get; }
    IGenericRepository<Booking, Guid> BookingRepository { get; }
    IGenericRepository<Campus, Guid> CampusRepository { get; }
    IGenericRepository<Department, Guid> DepartmentRepository { get; }
    IGenericRepository<Room, Guid> RoomRepository { get; }
    IGenericRepository<Slot, Guid> SlotRepository { get; }
    Task<int> SaveChangesAsync(bool trackAudit = true, bool trackSoftDelete = true);
}