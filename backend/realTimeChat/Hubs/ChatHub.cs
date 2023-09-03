using System;
using System.Threading.Tasks;
using realTimeServices.Dtos;
using realTimeServices.Services;
using Microsoft.AspNetCore.SignalR;

namespace realTimeServices.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ChatService _chatService;
        public ChatHub(ChatService chatService)
        {
            _chatService = chatService;
        }

        public override async Task OnConnectedAsync()
        {
            var UserID = Context.GetHttpContext().Request.Query["UserID"];
        //    Console.WriteLine(UserID);
          if(UserID.ToString() != "undefined" &&  UserID.ToString() != "" && UserID.ToString() is not null){
            var GetAllRoomIdes = _chatService.AddAndGetUserRooms(UserID); // Note online users
            // loop
            foreach (var uid in GetAllRoomIdes)
            {
              await Groups.AddToGroupAsync(Context.ConnectionId, uid);
            }
            // endof loop
            Console.WriteLine($"user connected id {UserID}");
            await Clients.Caller.SendAsync("UserConnected");
           }


        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {

            var userid = _chatService.GetUserIdByConnectionId(Context.ConnectionId);
              if(userid is not null){
                Console.WriteLine($"uid from conid {userid}");
                var uidlistfromRooms =  _chatService.RemoveUserFromList(userid);
                if(uidlistfromRooms is not null){
                    foreach (var uid in uidlistfromRooms)
                      {
                       await Groups.RemoveFromGroupAsync(Context.ConnectionId, uid); // suki
                       if (userid != uid){
                         await DisplayOnlineOtherUsers(uid);
                       }
                  }
              }
              
              await base.OnDisconnectedAsync(exception);
            }


        }

        public async Task AddUserConnectionId(string id){ // added to rooms
         if(id != "" &&  id != "undefined" && id is not null){
            _chatService.AddUserConnectionId(id, Context.ConnectionId);
            await DisplayOnlineOtherUsers(id);
         }
        }

 
        private async Task DisplayOnlineOtherUsers(string id){
            Console.WriteLine($"Diplay online called id {id}");
           if(id != "" && id != "undefined" && id is not null){
            var uidlistfromRooms = _chatService.GetOnlyUserRooms(id); 
            foreach (string uid in uidlistfromRooms)
            {
                // if (uid != id){
                 var onlineUsersx = _chatService.GetOnlineUsers(uid);
                 await Clients.Groups(uid).SendAsync("OnlineUsers" + uid, onlineUsersx);
                // }
            }
           }
        }


        public async Task RecivePrivateMessage(MessageDto message)
        {
            string privateGroupName = GetPrivateGroupName(message.sender, message.recever);
            await Groups.AddToGroupAsync(Context.ConnectionId, privateGroupName);
            var toConnectionId = _chatService.GetConnectionIdByUser(message.recever);
            await Groups.AddToGroupAsync(toConnectionId, privateGroupName);

            // opening private chatbox from the other end user
            await Clients.Client(toConnectionId).SendAsync("OpenPrivateChat", message);
            await Clients.Group(privateGroupName).SendAsync("NewPrivateMessage",message);
            _chatService.SaveMessageToDb(message);
        }

        public async Task RemovePrivateChat(string sender, string recever)
        {
            string privateGroupName = GetPrivateGroupName(sender, recever);
            await Clients.Group(privateGroupName).SendAsync("ClosePrivateChat");

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, privateGroupName);
            var toConnectionId = _chatService.GetConnectionIdByUser(recever);
            await Groups.RemoveFromGroupAsync(toConnectionId, privateGroupName);
        }

        private string GetPrivateGroupName(string sender, string recever)
        {
            // sender: john, recever: devid "divid-hohn"
            var stringCompare = string.CompareOrdinal(sender, recever) < 0;
            return stringCompare ? $"{sender}-{recever}" : $"{recever}-{sender}";
        }

    }
}