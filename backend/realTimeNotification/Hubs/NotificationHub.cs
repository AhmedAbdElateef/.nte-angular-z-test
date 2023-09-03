using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;


     public class NotificationHub : Hub
     {
        public async Task JoinChannel(string userId)
         {
             await Groups.AddToGroupAsync(Context.ConnectionId, userId);
         }

        public async Task SendNotification(string userId, string notification)
        {
           await Clients.Group(userId).SendAsync("ReceiveNotification", notification);
        }
     }