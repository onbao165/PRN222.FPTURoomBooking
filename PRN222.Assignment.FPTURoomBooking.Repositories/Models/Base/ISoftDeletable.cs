namespace PRN222.Assignment.FPTURoomBooking.Repositories.Models.Base;

public interface ISoftDeletable
{
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}