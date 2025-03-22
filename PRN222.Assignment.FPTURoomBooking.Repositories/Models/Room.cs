using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using PRN222.Assignment.FPTURoomBooking.Repositories.Models.Base;

namespace PRN222.Assignment.FPTURoomBooking.Repositories.Models;

public class Room : AuditableEntity
{
    [MaxLength(255)] public string Name { get; set; } = null!;
    [MaxLength(2000)] public string? Description { get; set; }
    [ForeignKey(nameof(Department))] public Guid DepartmentId { get; set; } // Room belongs to a department
    public virtual Department Department { get; set; } = null!;
    public virtual ICollection<RoomSlot> RoomSlots { get; set; } = [];

    public static Expression<Func<Room, object>> GetSortValue(string orderBy)
    {
        return orderBy switch
        {
            "name" => room => room.Name,
            "id" => room => room.Id,
            _ => room => room.UpdatedAt ?? room.CreatedAt
        };
    }
}