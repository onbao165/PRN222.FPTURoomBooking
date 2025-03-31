using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using PRN222.Assignment.FPTURoomBooking.Repositories.Models.Base;

namespace PRN222.Assignment.FPTURoomBooking.Repositories.Models;

public class Room : AuditableEntity
{
    [MaxLength(255)] public string Name { get; set; } = null!;
    [MaxLength(2000)] public string? Description { get; set; }
    [ForeignKey(nameof(Campus))] public Guid CampusId { get; set; } // Room belongs to a campus
    public virtual Campus Campus { get; set; } = null!;
    public virtual ICollection<Slot> Slots { get; set; } = [];

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