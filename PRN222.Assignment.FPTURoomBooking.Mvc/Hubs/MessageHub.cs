using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;

namespace PRN222.Assignment.FPTURoomBooking.Mvc.Hubs;


public class MessageHub : Hub<IMessageHubClient>
{
    // public async Task JoinGroup(string groupName)
    // {
    //     await this.Groups.AddToGroupAsync(this.Context.ConnectionId, groupName);
    //     await this.Clients.Group(groupName).SendAsync("Send", $"{this.Context.ConnectionId} joined {groupName}");
    // }
    public async Task SendBookingStatusUpdate()
    {
        await Clients.All.ReceiveBookingStatusUpdate();
    }

    public async Task JoinDepartmentGroup(string departmentId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, departmentId);
    }

    public async Task LeaveDepartmentGroup(string departmentId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, departmentId);
    }
}