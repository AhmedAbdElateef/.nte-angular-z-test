import { Injectable } from '@angular/core';
import {Observable} from 'rxjs'
import {HttpClient} from '@angular/common/http'

import { Notification } from '../types/Notification';
import { environment } from 'src/environments/environment';




@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  private apiUrl = environment.APIUrl+'notification'
  NotifyServiceData:Array<Notification>=[]
  userid : string =  JSON.parse(localStorage.getItem('profile') as string)?.result?._id;
  IsThereIsUnreadedNotify : number = 0;
  
  constructor(private http:HttpClient) { }


  checkifweHaveUnreadedNotifications(){

    this.GetUserNotification().subscribe({
      next:()=>{
        this.NotifyServiceData.forEach(noty=>{
          if(noty.isreded == false){
            this.IsThereIsUnreadedNotify = this.IsThereIsUnreadedNotify + 1;
            console.log("notservice", this.IsThereIsUnreadedNotify)
          }
        })
      }
    })
  }

  GetUserNotification(): Observable<Array<Notification>> {
    const notify = this.http.get<any>(`${this.apiUrl}/${this.userid}`);
    notify.subscribe({
      next:({ notifications }) => {
        this.NotifyServiceData = notifications;
      }
    })
    return notify;
  }

  MarkUserNotificationAsReaded(): Observable<Array<Notification>> {
    const notify = this.http.get<any>(`${this.apiUrl}/mark-notification-asreaded?id=${this.userid}`);
    notify.subscribe({
      next:({ notifications }) => {
        this.NotifyServiceData = notifications;
      }
    })
    return notify;
  }


}
