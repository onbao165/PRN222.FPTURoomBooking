using PRN222.Assignment.FPTURoomBooking.Repositories.Models;
using PRN222.Assignment.FPTURoomBooking.Services.Utils;

namespace PRN222.Assignment.FPTURoomBooking.Services.Models.Booking;

public class GetBookingModel : PaginationParams
{
    public Guid? AccountId { get; set; }
    public Guid? ManagerId { get; set; }
    public BookingStatus? Status { get; set; }
}