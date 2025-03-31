using Microsoft.AspNetCore.SignalR;

namespace PRN222.Assignment.FPTURoomBooking.Mvc.Hubs;

public class MessageHub : Hub<IMessageHubClient>
{
    // public async Task JoinGroup(string groupName)
    // {
    //     await this.Groups.AddToGroupAsync(this.Context.ConnectionId, groupName);
    //     await this.Clients.Group(groupName).SendAsync("Send", $"{this.Context.ConnectionId} joined {groupName}");
    // }
    public async Task SendBookingStatusUpdate(string accountId, string departmentId)
    {
        // Send notification to the user and all managers in the department
        await Clients.User(accountId).ReceiveBookingStatusUpdate();
        await Clients.Group(departmentId).ReceiveBookingStatusUpdate();
    }
    
    public async Task SendCampusUpdate()
    {
        await Clients.All.ReceiveCampusUpdate();
    }
    
    public async Task SendDepartmentUpdate()
    {
        await Clients.All.ReceiveDepartmentUpdate();
    }
    
    public async Task SendRoomUpdate()
    {
        await Clients.All.ReceiveRoomUpdate();
    }

    public async Task JoinDepartmentGroup(string departmentId)
    {
        Console.WriteLine($"{Context.ConnectionId} joined department group {departmentId}");
        await Groups.AddToGroupAsync(Context.ConnectionId, departmentId);
    }

    public async Task LeaveDepartmentGroup(string departmentId)
    {
        Console.WriteLine($"{Context.ConnectionId} left department group {departmentId}");
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, departmentId);
    }
}