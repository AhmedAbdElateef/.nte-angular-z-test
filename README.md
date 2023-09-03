
# to run

# docker-compose up -d


# to take it down

# docker-compose down 


# visit ui

@https://localhost/


# visit swegger docs

@https://localhost:7288/swagger


######### Realtime SignlaR ############

# Notification

[  RealTimeNotificationUrl: 'https://localhost:8090/'  ]



# code with TS

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


# RealTime Chat

[  RealTimeUrl: 'https://localhost:8000/']

# code with TS

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






