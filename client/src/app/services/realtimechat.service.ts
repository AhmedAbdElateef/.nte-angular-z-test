
import { EventEmitter, Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import { Message } from '../types/Message';
import { UsersService } from './users.service';

@Injectable({
  providedIn: 'root'
})
export class RealtimechatService {

  private chatConnection?: HubConnection;
  onlineUsers: string[] = [];
  privateMessages: Message[] = [];
  userid : string = "";
  TryConnectedTime = 0;
  onlineUsersUpdated: EventEmitter<string[]> = new EventEmitter<string[]>();
  privateMessagesUpdated: EventEmitter<Message[]> = new EventEmitter<Message[]>();


  async StartUserConnection(){
    this.userid =  JSON.parse(localStorage.getItem('profile') as string)?.result?._id;
    
    if(this.TryConnectedTime ==0 && this.userid){
      console.log("Realtime Chat Services Start user Called!!")
      
      await this.createChatConnection();

      this.TryConnectedTime = this.TryConnectedTime+1;

      this.onlineUsersUpdated.subscribe((onlineUsers) => {
        console.log("Realtime chat service online users", onlineUsers);
        this.onlineUsers = onlineUsers;
      });
    
    }
  }


  async createChatConnection(){
      this.chatConnection = new HubConnectionBuilder()
      .withUrl(`${environment.RealTimeUrl}hubs/chat?UserID=${this.userid}`).withAutomaticReconnect().build();

    this.chatConnection.start().catch(error => {
      console.log(error);
    })


    this.chatConnection.on('UserConnected', () => {
      console.log('UserConnected')
      if(this.userid){
        this.addUserConnectionId(this.userid);
      }
    })

    this.chatConnection.on('OnlineUsers' + this.userid, (onlineUsers) => {
      console.log('OnlineUsers')
      // this.onlineUsers = [...onlineUsers];
      // this.onlineUsersUpdated.emit(this.onlineUsers); // Emit the event with updated onlineUsers     
      const uniqueUsers:string[] = Array.from(new Set(onlineUsers)); // Remove duplicates
      this.onlineUsers = [...uniqueUsers];
      this.onlineUsersUpdated.emit(this.onlineUsers); // Emit the event with updated onlineUsers
    });


    this.chatConnection.on('OpenPrivateChat', (newMessage: Message)=>{
      console.log('OpenPrivateChat')
      this.privateMessages = [...this.privateMessages, newMessage];
    })

    this.chatConnection.on('NewPrivateMessage', (newMessage: Message)=>{
      this.privateMessages = [...this.privateMessages, newMessage];
      this.privateMessagesUpdated.emit(this.privateMessages); // Emit the event with updated onlineUsers

      console.log('NewPrivateMessage', this.privateMessages)

    })


    this.chatConnection.on('ClosePrivateChat', ()=>{
      console.log('ClosePrivateChat')
      this.privateMessages = [];
    })

  }

  ////////////////////////////

  stopChatConnection(){
    this.TryConnectedTime = 0;
    this.chatConnection?.stop().catch(error => console.log(error));
  }

  async addUserConnectionId(userid: string){
    return this.chatConnection?.invoke('AddUserConnectionId', userid)
      .catch(error => console.log(error));
  }

  async sendPrivateMessage(message:Message){


    return this.chatConnection?.invoke('RecivePrivateMessage', message)
    .catch(error=> console.log(error));

  }

  async closePrivateChatMessage(otherUser: string){
    return this.chatConnection?.invoke("RemovePrivateChat", this.userid, otherUser)
     .catch(error => console.log(error));
  }


}