namespace PRN222.Assignment.FPTURoomBooking.Repositories.Models.Base;

public interface IEntity<TKey>
{
    TKey Id { get; set; }
}