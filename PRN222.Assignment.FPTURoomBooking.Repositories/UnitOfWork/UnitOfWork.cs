using PRN222.Assignment.FPTURoomBooking.Repositories.Models;
using PRN222.Assignment.FPTURoomBooking.Repositories.Repositories;

namespace PRN222.Assignment.FPTURoomBooking.Repositories.UnitOfWork;

/// <summary>
/// Unit of Work implementation
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private bool _disposed;

    private IGenericRepository<Account, Guid>? _accountRepository;
    private IGenericRepository<Booking, Guid>? _bookingRepository;
    private IGenericRepository<Campus, Guid>? _campusRepository;
    private IGenericRepository<Department, Guid>? _departmentRepository;
    private IGenericRepository<Room, Guid>? _roomRepository;
    private IGenericRepository<RoomSlot, Guid>? _roomSlotRepository;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IGenericRepository<Account, Guid> AccountRepository =>
        _accountRepository ??= new GenericRepository<Account, Guid>(_context);

    public IGenericRepository<Booking, Guid> BookingRepository =>
        _bookingRepository ??= new GenericRepository<Booking, Guid>(_context);

    public IGenericRepository<Campus, Guid> CampusRepository =>
        _campusRepository ??= new GenericRepository<Campus, Guid>(_context);

    public IGenericRepository<Department, Guid> DepartmentRepository =>
        _departmentRepository ??= new GenericRepository<Department, Guid>(_context);

    public IGenericRepository<Room, Guid> RoomRepository =>
        _roomRepository ??= new GenericRepository<Room, Guid>(_context);

    public IGenericRepository<RoomSlot, Guid> RoomSlotRepository =>
        _roomSlotRepository ??= new GenericRepository<RoomSlot, Guid>(_context);


    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _context.Dispose();
        }

        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}