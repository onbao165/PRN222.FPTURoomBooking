using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using PRN222.Assignment.FPTURoomBooking.Repositories.Models.Base;

namespace PRN222.Assignment.FPTURoomBooking.Repositories.Models;

public class Campus : AuditableEntity
{
    [MaxLength(255)] public string Name { get; set; } = null!;
    [MaxLength(2000)] public string? Address { get; set; }
    public virtual ICollection<Department> Departments { get; set; } = []; // Khu A, Khu B, Khu C
    
    public static Expression<Func<Campus, object>> GetSortValue(string orderBy)
    {
        return orderBy switch
        {
            "name" => campus => campus.Name,
            "id" => campus => campus.Id,
            _ => campus => campus.UpdatedAt ?? campus.CreatedAt
        };
    }
}