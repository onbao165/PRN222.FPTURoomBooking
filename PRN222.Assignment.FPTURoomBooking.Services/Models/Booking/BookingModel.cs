using PRN222.Assignment.FPTURoomBooking.Repositories.Models;
using PRN222.Assignment.FPTURoomBooking.Services.Models.Account;

namespace PRN222.Assignment.FPTURoomBooking.Services.Models.Booking;

public class BookingModel
{
    public Guid Id { get; set; }
    public DateTime BookingDate { get; set; }
    public BookingStatus Status { get; set; }
    public Guid AccountId { get; set; } // Booking belongs to an account
    public Guid? ManagerId { get; set; } // Booking is approved by an account
    public AccountModel Account { get; set; }
}