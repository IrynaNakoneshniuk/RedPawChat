using Microsoft.AspNetCore.SignalR;


namespace RedPawChat.Server
{
    public class ChatHub :Hub
    {
        public async Task SendMessage(Guid conversationId, string userName, string message)
        {
            await Clients.Group(conversationId.ToString()).SendAsync("ReceiveMessage", userName, message);
        }

        public async Task JoinDialog(string dialogId, string userName)
        {
            // Додати користувача до діалогу
            await Groups.AddToGroupAsync(Context.ConnectionId, dialogId);

            // Зберегти мапу між ідентифікатором користувача та діалогом

            // Відправити повідомлення про приєднання нового користувача
            await Clients.Group(dialogId).SendAsync("UserJoined", userName);
        }

        public async Task LeaveDialog(string userName, Guid dialogId )
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, dialogId.ToString());

            // Відправити повідомлення про виїзд користувача
            await Clients.Group(dialogId.ToString()).SendAsync("UserLeft", userName);
        }
    }
}
