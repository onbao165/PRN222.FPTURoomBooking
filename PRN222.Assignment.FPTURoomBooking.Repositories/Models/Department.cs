using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using PRN222.Assignment.FPTURoomBooking.Repositories.Models.Base;

namespace PRN222.Assignment.FPTURoomBooking.Repositories.Models;

public class Department : AuditableEntity
{
    [MaxLength(255)] public string Name { get; set; } = null!; // Khu A, Khu B, Khu C
    [MaxLength(2000)] public string? Description { get; set; }
    [ForeignKey(nameof(Campus))] public Guid CampusId { get; set; } // Department belongs to a campus
    public virtual Campus Campus { get; set; } = null!;
    public virtual ICollection<Account> Accounts { get; set; } = []; // Department has many managers
    public virtual ICollection<Room> Rooms { get; set; } = [];

    public static Expression<Func<Department, object>> GetSortValue(string orderBy)
    {
        return orderBy switch
        {
            "name" => department => department.Name,
            "id" => department => department.Id,
            _ => department => department.UpdatedAt ?? department.CreatedAt
        };
    }
}