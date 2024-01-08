using Microsoft.AspNetCore.SignalR;


namespace RedPawChat.Server
{
    public class ChatHub :Hub
    {
        public async Task SendMessage(string conversationId, string userName, string message, string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, conversationId.ToString());
            await Clients.Group(conversationId.ToString()).SendAsync("SendMessage",conversationId, userName, message,userId);
        }

        public async Task JoinDialog(string dialogId, string userName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, dialogId);

            await Clients.Group(dialogId).SendAsync("UserJoined", userName);
        }

        public async Task LeaveDialog(string userName,string dialogId )
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, dialogId.ToString());

            await Clients.Group(dialogId.ToString()).SendAsync("UserLeft", userName);
        }
    }
}
