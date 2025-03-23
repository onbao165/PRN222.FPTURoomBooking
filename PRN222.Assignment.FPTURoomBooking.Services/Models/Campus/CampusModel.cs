namespace PRN222.Assignment.FPTURoomBooking.Services.Models.Campus;

public class CampusModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Address { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}