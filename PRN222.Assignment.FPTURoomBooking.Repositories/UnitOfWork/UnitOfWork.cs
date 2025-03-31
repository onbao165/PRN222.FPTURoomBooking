using Microsoft.EntityFrameworkCore;
using PRN222.Assignment.FPTURoomBooking.Repositories.Models;
using PRN222.Assignment.FPTURoomBooking.Repositories.Models.Base;
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
    private IGenericRepository<Slot, Guid>? _slotRepository;

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
    
    public IGenericRepository<Slot, Guid> SlotRepository =>
        _slotRepository ??= new GenericRepository<Slot, Guid>(_context);

    public async Task<int> SaveChangesAsync(bool trackAudit = true, bool trackSoftDelete = true)
    {
        if (trackAudit)
        {
            TrackAuditChanges();
        }

        if (trackSoftDelete)
        {
            TrackSoftDeleteChanges();
        }

        return await _context.SaveChangesAsync();
    }

    private void TrackAuditChanges()
    {
        var entries = _context.ChangeTracker.Entries()
            .Where(e => e.Entity is AuditableEntity<Guid> or AuditableEntity<int> &&
                        e.State is EntityState.Added or EntityState.Modified);

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    switch (entry.Entity)
                    {
                        case AuditableEntity<Guid> guidEntity:
                            guidEntity.Id = Guid.NewGuid();
                            guidEntity.CreatedAt = DateTime.UtcNow;
                            break;
                        case AuditableEntity<int> intEntity:
                            intEntity.CreatedAt = DateTime.UtcNow;
                            break;
                    }

                    break;

                case EntityState.Modified:
                    switch (entry.Entity)
                    {
                        case AuditableEntity<Guid> guidEntity:
                            guidEntity.UpdatedAt = DateTime.UtcNow;
                            entry.Property(nameof(AuditableEntity<Guid>.CreatedAt)).IsModified = false;
                            break;
                        case AuditableEntity<int> intEntity:
                            intEntity.UpdatedAt = DateTime.UtcNow;
                            entry.Property(nameof(AuditableEntity<int>.CreatedAt)).IsModified = false;
                            break;
                    }

                    break;
            }
        }
    }

    private void TrackSoftDeleteChanges()
    {
        var entries = _context.ChangeTracker.Entries()
            .Where(e => e is { Entity: ISoftDeletable, State: EntityState.Deleted });

        foreach (var entry in entries)
        {
            entry.State = EntityState.Modified;
            if (entry.Entity is not ISoftDeletable softDeletable) continue;
            softDeletable.IsDeleted = true;
            softDeletable.DeletedAt = DateTime.UtcNow;
        }
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