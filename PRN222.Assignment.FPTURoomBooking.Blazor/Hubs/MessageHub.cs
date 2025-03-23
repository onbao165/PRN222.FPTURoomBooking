using Microsoft.AspNetCore.SignalR;

namespace PRN222.Assignment.FPTURoomBooking.Blazor.Hubs;

public class MessageHub : Hub
{
    
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }
    
    public async Task SendBookingStatusUpdate()
    {
        await Clients.All.SendAsync("ReceiveBookingStatusUpdate");
    }
}