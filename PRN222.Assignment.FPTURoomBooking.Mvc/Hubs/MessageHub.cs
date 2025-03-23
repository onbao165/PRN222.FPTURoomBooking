using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace PRN222.Assignment.FPTURoomBooking.Mvc.Hubs;

[Authorize(Roles = "Manager, User, Admin")]
public class MessageHub : Hub<IMessageHubClient>
{
    public async Task JoinDepartmentGroup(string departmentId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, departmentId);
    }

    public async Task LeaveDepartmentGroup(string departmentId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, departmentId);
    }
}