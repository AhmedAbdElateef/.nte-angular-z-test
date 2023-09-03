using System;
using realTimeServices.Dtos;

using System.Collections.Generic;
using System.Linq;

namespace realTimeServices.Services
{
    public class ChatService
    {
        private static readonly Dictionary<string, string> Users = new Dictionary<string, string>();        

        public class Rooms
        {
         public  Dictionary<string, List<string>> Room {get; set; } = new Dictionary<string, List<string>>();
        }

        Rooms room = new Rooms();
        

        public void SaveMessageToDb(MessageDto message){
                   var client = new RealTimeChatClient();
                   client.SendMessage(message.content,message.sender, message.recever);
        }
        
        // my creation
        public List<string> AddAndGetUserRooms(string userid){

          // TODO :: Using GRPC get user followers & Following
          var client = new RealTimeChatClient();
          List<string> uides = new List<string>();

          if (userid != "undefined" && userid != "" && userid is not null ){ 
              var uidesx = client.GetUsersIdes(userid);
              uidesx.ForEach((id)=>{
                // Console.WriteLine($" chat ser response data {id}");
                if (room.Room.ContainsKey(id)) 
                    {
                      uides.Add(id); 
                    }
              });
              

              if (room.Room.ContainsKey(userid)){
                room.Room[userid] = uides;
              } else {
                room.Room.Add(userid, uides);
              }

              // add userid to Other user rooms
              foreach (var uid in uides)
              {
                if(room.Room.ContainsKey(uid)){
                   room.Room[uid].Add(userid);
                }
              }

              uides.Add(userid);

              }

          return uides;
        }
       public List<string> RemoveUserFromList(string userid)
        { 
            // 
          if (userid != "undefined" && userid != "" && userid is not null ){
            lock (Users)
            {
                if (Users.ContainsKey(userid))
                {
                    Users.Remove(userid);
                }
            }


            var uides = room.Room[userid];  // mohamed suki hady
            var removedlist = room.Room[userid];  // mohamed suki hady


            foreach (var uid in uides.ToArray())
              {
                if(room.Room.ContainsKey(uid)){
                   room.Room[uid].Remove(userid);
                }
              }
            // 
        
            if (room.Room.ContainsKey(userid))
            {
              room.Room.Remove(userid);
            }

          removedlist.Add(userid);
          return removedlist;
          
          } else {
            return null;
          }

        }


        public string[] GetOnlineUsers(string id)
        {
          try{
           var online = room.Room[id].ToArray();
           return online;
          }catch{
              return null;
          }

        }

        public List<string> GetOnlyUserRooms(string userid){
           try{
            List<string> uidesList = room.Room[userid];
            return uidesList;
           } catch {
            return null;
           }

        }


        // connection method
        public void AddUserConnectionId(string userid, string connectionId)
        {
          if(userid != "undefined" &&
             userid != ""  &&
             userid is not null &&
             connectionId != "undefined" &&
             connectionId != "" && 
             connectionId is not null){

            lock(Users)
            {
                if (!Users.ContainsKey(userid))
                {
                    Users[userid] = connectionId;
                }
            }
          } 
        }

        
        // get user by connection id
        public string GetUserIdByConnectionId(string connectionId)
        {
          if(connectionId != "undefined" && connectionId != ""  && connectionId is not null){
            lock(Users)
            {
                return Users.Where(x => x.Value == connectionId).Select(x => x.Key).FirstOrDefault();
            }
          } else {
            return "";
          }
        }

        // get connection id by user
        public string GetConnectionIdByUser(string user)
        {
         if(user != "undefined" && user != "")
          {
            lock(Users)
            {
                return Users.Where(x => x.Key == user).Select(x => x.Value).FirstOrDefault();
            }
          } else {
            return "";
          }
        }



    }
}