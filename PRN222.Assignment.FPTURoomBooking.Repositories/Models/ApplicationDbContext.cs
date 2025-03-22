using Microsoft.EntityFrameworkCore;

namespace PRN222.Assignment.FPTURoomBooking.Repositories.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Account> Accounts { get; set; } = null!;
    public DbSet<Booking> Bookings { get; set; } = null!;
    public DbSet<Campus> Campuses { get; set; } = null!;
    public DbSet<Department> Departments { get; set; } = null!;
    public DbSet<Room> Rooms { get; set; } = null!;
    public DbSet<RoomSlot> RoomSlots { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable(nameof(Account));
            entity.Property(p => p.Role)
                .HasConversion(
                    v => v.ToString(),
                    v => (AccountRole)Enum.Parse(typeof(AccountRole), v));
        });
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.ToTable(nameof(Booking));
            entity.Property(p => p.Status)
                .HasConversion(
                    v => v.ToString(),
                    v => (BookingStatus)Enum.Parse(typeof(BookingStatus), v));
        });
        modelBuilder.Entity<Campus>(entity => { entity.ToTable(nameof(Campus)); });
        modelBuilder.Entity<Department>(entity => { entity.ToTable(nameof(Department)); });
        modelBuilder.Entity<Room>(entity => { entity.ToTable(nameof(Room)); });
        modelBuilder.Entity<RoomSlot>(entity =>
        {
            entity.ToTable(nameof(RoomSlot));
            entity.Property(p => p.TimeSlot)
                .HasConversion(
                    v => v.ToString(),
                    v => (TimeSlot)Enum.Parse(typeof(TimeSlot), v));
        });
    }
}