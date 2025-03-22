using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using PRN222.Assignment.FPTURoomBooking.Repositories.Models.Base;

namespace PRN222.Assignment.FPTURoomBooking.Repositories.Models;

public class RoomSlot : AuditableEntity
{
    [ForeignKey(nameof(Room))] public Guid RoomId { get; set; } // RoomSlot belongs to a room
    [ForeignKey(nameof(Booking))] public Guid BookingId { get; set; } // RoomSlot belongs to a booking
    public TimeSlot TimeSlot { get; set; }
    public virtual Room Room { get; set; } = null!;
    public virtual Booking Booking { get; set; } = null!;
    public static Expression<Func<RoomSlot, object>> GetSortValue(string orderBy)
    {
        return orderBy switch
        {
            "timeSlot" => roomSlot => roomSlot.TimeSlot,
            "id" => roomSlot => roomSlot.Id,
            _ => roomSlot => roomSlot.UpdatedAt ?? roomSlot.CreatedAt
        };
    }
}

public enum TimeSlot
{
    Slot1 = 1, // 7:00 - 9:15
    Slot2 = 2, // 9:30 - 11:45
    Slot3 = 3, // 12:30 - 14:45
    Slot4 = 4, // 15:00 - 17:15
    Slot5 = 5 // 17:45 - 20:00
}