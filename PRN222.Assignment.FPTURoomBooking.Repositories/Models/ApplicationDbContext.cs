using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PRN222.Assignment.FPTURoomBooking.Repositories.Models.Base;

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
    public DbSet<Slot> Slots { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }
    
    private static void SoftDeleteFilter(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (!typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType)) continue;
            var parameter = Expression.Parameter(entityType.ClrType, "p");
            var property = Expression.Property(parameter, nameof(ISoftDeletable.IsDeleted));
            var condition = Expression.Equal(property, Expression.Constant(false));
            var lambda = Expression.Lambda(condition, parameter);

            modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        SoftDeleteFilter(modelBuilder);
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
        modelBuilder.Entity<Slot>(entity =>
        {
            entity.ToTable(nameof(Slot));
        });
    }
}