using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using PRN222.Assignment.FPTURoomBooking.Repositories.Models.Base;

namespace PRN222.Assignment.FPTURoomBooking.Repositories.Models;

public class Booking : AuditableEntity
{
    public DateTime BookingDate { get; set; }
    public BookingStatus Status { get; set; }
    [ForeignKey(nameof(Account))] public Guid AccountId { get; set; } // Booking belongs to an account
    public Guid? ManagerId { get; set; } // Booking is approved by an account
    public virtual Account Account { get; set; } = null!;
    public virtual ICollection<Slot> Slots { get; set; } = [];

    public static Expression<Func<Booking, object>> GetSortValue(string orderBy)
    {
        return orderBy switch
        {
            "bookingDate" => booking => booking.BookingDate,
            "status" => booking => booking.Status,
            "id" => booking => booking.Id,
            _ => booking => booking.UpdatedAt ?? booking.CreatedAt
        };
    }
}

public enum BookingStatus
{
    Pending = 1,
    Approved = 2,
    Rejected = 3,
    Cancelled = 4
}