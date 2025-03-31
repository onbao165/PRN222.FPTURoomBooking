using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using PRN222.Assignment.FPTURoomBooking.Repositories.Models.Base;

namespace PRN222.Assignment.FPTURoomBooking.Repositories.Models;

public class Slot : AuditableEntity
{
    [ForeignKey(nameof(Room))] public Guid RoomId { get; set; } // RoomSlot belongs to a room
    [ForeignKey(nameof(Booking))] public Guid BookingId { get; set; } // RoomSlot belongs to a booking
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public virtual Room Room { get; set; } = null!;
    public virtual Booking Booking { get; set; } = null!;
    public static Expression<Func<Slot, object>> GetSortValue(string orderBy)
    {
        return orderBy switch
        {
            "id" => slot => slot.Id,
            _ => slot => slot.StartTime
        };
    }
}