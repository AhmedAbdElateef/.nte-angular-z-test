import { Component, OnInit } from '@angular/core';
import { NotificationService } from '../services/notification.service';
import { Notification } from '../types/Notification';
import { UsersService } from '../services/users.service';
import { SignalRService } from '../services/realtime-notifiy.service';
import { Subscription } from 'rxjs';
import { Router } from '@angular/router';

@Component({
  selector: 'app-notification',
  templateUrl: './notification.component.html',
  styleUrls: ['./notification.component.css']
})
export class NotificationComponent implements OnInit {
// notifications.ts

notify: Notification[] = [];

// realtime
private notificationSubscription!: Subscription;


  constructor(    public notifyService: NotificationService,
                  public userService: UsersService,
                  private signalRService: SignalRService,  
                  private router:Router  ) { }

  ngOnInit(): void {

    this.notifyService.GetUserNotification().subscribe({
      next:()=>{
        console.log("notifyconponnets print notifiy", this.notifyService.NotifyServiceData);
        this.notify = this.notifyService.NotifyServiceData;
        setTimeout(() => {
          this.MarkNotifiyAsReaded();
        }, 1000);
      }
    })


    this.notificationSubscription = this.signalRService.receiveNotification().subscribe((notification: any) => {
      // console.log("notify componnot--------|||||||||||", notification);
      this.notify.unshift(notification);
      setTimeout(() => {
        this.MarkNotifiyAsReaded();
      }, 1000);
    });
    
  }



  MarkNotifiyAsReaded() {
   this.notifyService.MarkUserNotificationAsReaded().subscribe({
    next:()=>{
      this.notify.forEach(notify =>{
        notify.isreded = true;
        this.notifyService.IsThereIsUnreadedNotify = 0;
        // this.signalRService.notifyideslist = [];
      })
    }
   })
  }


  MoveToTheSourceOfThePost( notification:Notification){
    // this.router.navigate(['/search'], { queryParams: { data } })
    if(notification.deatils?.includes("Post")){
      this.router.navigate(['/EditShowPost' + `/${notification?.targetid}`])
    } else if(notification.deatils?.includes("Following")){
      this.router.navigate(['/profile' + `/${notification?.targetid}`])
    }
  }

  ngOnDestroy(): void {
    this.notificationSubscription.unsubscribe();
    this.signalRService.stop();
  }
}
