import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { NotificationService } from './notification.service';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private hubConnection!: HubConnection;
  userid : string =  JSON.parse(localStorage.getItem('profile') as string)?.result?._id;
  TryConnectedTime = 0;
  notifyideslist : string[]= [];
  constructor(private notService: NotificationService) {
      
      this.hubConnection = new HubConnectionBuilder()
      .withUrl(`${environment.RealTimeNotificationUrl}hubs/Notifications`)
      .build();  
  }


  async connect() {
    if(this.userid && this.TryConnectedTime == 0){
      this.TryConnectedTime = this.TryConnectedTime + 1;

      this.hubConnection.start().then(() => {
        if(this.userid){
          this.hubConnection.invoke('JoinChannel', this.userid);
        }
      });
    }
  }

  
  receiveNotification(): Observable<any> {
    return new Observable<any>((observer) => {
      this.hubConnection.on('ReceiveNotification', (notification: any) => {
        if(!this.notifyideslist.includes(notification.id) || this.notifyideslist.length == 0){
          this.notifyideslist.push(notification.id);
          this.notService.IsThereIsUnreadedNotify = this.notService.IsThereIsUnreadedNotify + 1;
        }
        observer.next(notification);
      });
    });
  }

  stop(): void {
    this.hubConnection.stop();
    this.TryConnectedTime = 0;
  }

}