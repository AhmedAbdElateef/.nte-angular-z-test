import {  Component, OnInit } from '@angular/core';
import { UsersService } from './services/users.service';
import { RealtimechatService } from './services/realtimechat.service';
import { SignalRService } from './services/realtime-notifiy.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  private notificationSubscription!: Subscription;

  constructor(public AppusersService: UsersService,
              public RealTimeNotification: SignalRService,
              public realtimeChat: RealtimechatService) { }
  
  async ngOnInit(): Promise<void> {
  
    this.AppusersService.UpdateAuth()
    this.AppusersService.isUserAuth()
    
    if(this.AppusersService.isAuthenticated){
       await this.realtimeChat.StartUserConnection();
       await this.RealTimeNotification.connect();
    }

    this.AppusersService.CheckIfUserIsAuthenticated.subscribe(async (isauth)=>{
      if(isauth){
        await this.realtimeChat.StartUserConnection();
        await this.RealTimeNotification.connect();

      }
    })


    
    this.notificationSubscription = this.RealTimeNotification
    .receiveNotification()
    .subscribe((notification: any) => {});
  }

  ngOnDestroy(): void {
    this.AppusersService.CheckIfUserIsAuthenticated.subscribe((isauth)=>{
      if(isauth){
        this.RealTimeNotification.stop();
        this.realtimeChat.stopChatConnection();
      }
    })
  }




}
