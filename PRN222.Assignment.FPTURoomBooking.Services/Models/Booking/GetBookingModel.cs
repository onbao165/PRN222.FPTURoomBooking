using PRN222.Assignment.FPTURoomBooking.Repositories.Models;
using PRN222.Assignment.FPTURoomBooking.Services.Utils;

namespace PRN222.Assignment.FPTURoomBooking.Services.Models.Booking;

public class GetBookingModel : PaginationParams
{
    public Guid? AccountId { get; set; }
    
    public Guid? RoomId { get; set; }
    public Guid? ManagerId { get; set; }
    public BookingStatus? Status { get; set; }
    public DateTime? BookingDate { get; set; }
    // Add date range properties
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
