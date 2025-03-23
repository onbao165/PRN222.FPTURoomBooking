namespace PRN222.Assignment.FPTURoomBooking.Mvc.Hubs;

public interface IMessageHubClient
{
    Task ReceiveNewBooking();
    Task ReceiveBookingStatusUpdate();
    Task ReceiveCampusUpdate();
    Task ReceiveDepartmentUpdate();
    Task ReceiveRoomUpdate();
}