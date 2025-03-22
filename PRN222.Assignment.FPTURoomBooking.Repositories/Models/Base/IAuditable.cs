namespace PRN222.Assignment.FPTURoomBooking.Repositories.Models.Base;

public interface IAuditable
{
    DateTime CreatedAt { get; set; }
    DateTime? UpdatedAt { get; set; }

}