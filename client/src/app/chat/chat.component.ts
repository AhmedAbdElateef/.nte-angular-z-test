import { Component, OnInit,  ViewChild, ElementRef  } from '@angular/core';
import { UsersService } from '../services/users.service';
import { User, UserInChat } from '../types/User';
import { ChatService } from '../services/chat.service';
import { Message, UnrededMsg } from '../types/Message';
import { RealtimechatService } from '../services/realtimechat.service';





@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit {
  
  @ViewChild('chatContent', { static: true }) chatContent: ElementRef | any;

  MainUser = this.userService.UserServiceData;

  StartPointGetingChat: number = 0;
 
  FrindsListUsers: UserInChat[] = [];
  onlineUsers: string[] = this.realtimeChat.onlineUsers;
  unRededMessageList : UnrededMsg[] = [];
  chatMessages: Message[] = [];

  newMessage: string = '';

  selectedUser: User =   {};

  constructor( private userService: UsersService
              ,private chatService: ChatService
              ,public realtimeChat: RealtimechatService) { }


  // ngOnDestroy(): void {
  //   this.realtimeChat.stopChatConnection();
  // }

  async ngOnInit(): Promise<void> {
    
        this.GetFollowingUsersList()
        await this.realtimeChat.StartUserConnection();
        this.getunrmsg();

        this.realtimeChat.onlineUsersUpdated.subscribe((onlineUsers) => {
          this.onlineUsers = onlineUsers;
        });
    


        this.realtimeChat.privateMessagesUpdated.subscribe((realtimeMessage) => {
          // --- update msg num --
          this.callunReadedMsg();
          this.getunrmsg();
          this.chatService.getUnreadedMessagesNum();
          // -----------
          const lastMessage = realtimeMessage[realtimeMessage.length - 1];
          if (lastMessage && (lastMessage.sender === this.selectedUser._id || lastMessage.recever === this.selectedUser._id))
          {
            this.chatMessages.push(lastMessage);
            setTimeout(() => {
              this.forceScroll();
            }, 100); // deley foreScroll for half secod
            }
        });



  }

  getunrmsg(){
    this.chatService.getUnreadedMessagesNum().subscribe({
      next:({messages})=>{
        this.unRededMessageList = messages;
        this.callunReadedMsg()
      }
    });
  }

  callunReadedMsg(){

    this.FrindsListUsers.forEach(user => {
      this.unRededMessageList.forEach(msg =>{
        if(String(msg.otherUserid) == String(user._id)){
          user.unReadedMessage = Number(msg.numOfUneadedMessages);
        }
      })
    });

  }


  clearUnnReadedMsg(suid:string){
    this.FrindsListUsers.forEach(user=>{
        if(String(user._id) == String(suid)){
          user.unReadedMessage = 0;
        }
    })
  }

  isUserOnline(userId: string): boolean {
    return this.onlineUsers.includes(userId);
  }


  GetFollowingUsersList(){
    this.userService.isUserAuth();
    var followersuides = this.MainUser?.followers
    var uides = this.MainUser?.following
    uides?.push(followersuides);
    if(uides){
      uides.forEach(id => {
       this.userService.GetUserData(id).subscribe({
        next:({user})=>{
          // console.log("chat users", user)
          var uichat:UserInChat = {
            _id:user._id,
            name: user.name,
            imageUrl:user.imageUrl, 
            unReadedMessage: 0
          }
          this.FrindsListUsers.push(uichat);
          this.callunReadedMsg();
          this.removeDuplicates();
        }
      })
      });
    }
  }


  removeDuplicates() {
    this.FrindsListUsers = this.FrindsListUsers.filter((user, index, self) =>
      index === self.findIndex((u) => u._id === user._id)
    );
  }


  forceScroll(){
    const element = this.chatContent.nativeElement;
    element.scrollTop = element.scrollHeight;
  }

  onScrollGetOldMessages() {
    const element = this.chatContent.nativeElement;
    if (element.scrollTop === 0) {
      var fuid = String(this.MainUser?._id)
      var suid = String(this.selectedUser?._id)
      
      this.chatService.getMessagesBetweenTwoUsers(this.StartPointGetingChat, fuid,suid).subscribe({
        next:({msgs})=>{
          console.log("ses", msgs);
          this.StartPointGetingChat += this.StartPointGetingChat;
          this.chatMessages.unshift(...msgs); 
          element.scrollTop = 10;
        }
      })


    }
  }


  selectUser(user: User) {
    this.selectedUser = user;
    var fuid = String(this.MainUser?._id)
    var suid = String(user?._id)
    this.StartPointGetingChat =0;

    this.chatService.getMessagesBetweenTwoUsers(this.StartPointGetingChat, fuid,suid).subscribe({
      next:({msgs})=>{
        // console.log("ses", msgs, "fuid", fuid, "suid", suid);
        this.StartPointGetingChat = this.StartPointGetingChat +1;
        this.chatMessages = msgs;
        this.clearUnnReadedMsg(suid)
        this.chatService.MarkmsgAsReaded(fuid, suid);
        setTimeout(() => {
              this.forceScroll();
        }, 100); // deley foreScroll for half second
      }
    })


  }



  sendMessage() {
    if (this.newMessage.trim() !== '') {
      var fuid = String(this.MainUser?._id)
      var suid = String(this.selectedUser?._id)

      const newMsg: Message = {
        content: this.newMessage,
        sender: fuid,
        recever: suid
        // timestamp: new Date()
      };


      // send realtime message
      if(this.isUserOnline(suid)){
        this.realtimeChat.sendPrivateMessage(newMsg);
        // this.realtimeChat.privateMessageInitiated = true;
        this.newMessage = '';
      } else {
              // send Offline message
      this.chatService.sendMessage(newMsg).subscribe({
        next:(res)=>{
          console.log('res form send',res)
          this.chatMessages.push(newMsg);
          this.newMessage = '';
        }
      })
      }


      

    }
  }




}


