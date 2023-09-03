import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UsersService } from '../services/users.service';
import { User } from '../types/User';
import { ChatService } from '../services/chat.service';
import { RealtimechatService } from '../services/realtimechat.service';
import { NotificationService } from '../services/notification.service';
import { SignalRService } from '../services/realtime-notifiy.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  value:string = ''
  // private notificationSubscription!: Subscription;


  constructor(private router:Router,
    public usersNavService: UsersService, 
    public notificationService: NotificationService,
    public RealTimenotificationService: SignalRService,
    public chatService: ChatService,
    public realtimeChat: RealtimechatService) { }

  async ngOnInit(): Promise<void> {   
      if( this.usersNavService.isAuthenticated ) {
        this.chatService.getUnreadedMessagesNum();
        await this.RealTimenotificationService.connect();
      };



      // this.notificationSubscription = this.RealTimenotificationService
      //                                 .receiveNotification()
      //                                 .subscribe((notification: any) => {});

      
    this.realtimeChat.privateMessagesUpdated.subscribe((realtimeMessage) => {
      if(realtimeMessage){
        this.chatService.getUnreadedMessagesNum();
      }
    });

    this.notificationService.checkifweHaveUnreadedNotifications();
  }

  Search(data:string){
    this.router.navigate(['/search'], { queryParams: { data } })
  }

  LogUserOut(){
    this.usersNavService.LogOut()
  }


}
