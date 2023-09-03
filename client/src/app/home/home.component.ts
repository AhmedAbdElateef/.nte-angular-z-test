import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { PostsService } from '../services/posts.service';
import { UsersService } from '../services/users.service';
import { RealtimechatService } from '../services/realtimechat.service';
import { SignalRService } from '../services/realtime-notifiy.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
 
export class HomeComponent implements OnInit {

  arr:Array<any> = []
  Pages:number = 0; 
  SelectedPage:number = 1
  constructor(public postsHomeService:PostsService, 
    public userHomeService:UsersService,
    public realtimeChat: RealtimechatService,
    public RealTimeNotification : SignalRService) { 
    this.arr = this.postsHomeService.PostsServiceData
  }

  async ngOnInit(): Promise<void> {
    console.log("home-componnet-called")
    await this.handlePageEvent(null)
    
    if(this.userHomeService.isAuthenticated){
      await this.realtimeChat.StartUserConnection();
      await this.RealTimeNotification.connect();
   }


    this.userHomeService.CheckIfUserIsAuthenticated.subscribe(async (auth)=>{
      if(auth){
        // this.realtimeChat.StartUserConnection();
        await this.realtimeChat.StartUserConnection();
        await this.RealTimeNotification.connect();
        // await this.handlePageEvent(null)
      }
    })



  }

  async handlePageEvent(page:PageEvent | null){
    console.log("handele event called")
    this.SelectedPage = page ? page.pageIndex + 1 : 1;
    this.postsHomeService.fetchPosts(this.SelectedPage, this.userHomeService.UserServiceData._id as string).subscribe({
      next:({currentPage, numberOfPages})=>{
        this.SelectedPage= currentPage
        this.Pages = numberOfPages
      }
    })
   // console.log('Selected Page',this.SelectedPage)
  }

  // ngOnDestroy(): void {
  //   this.realtimeChat.stopChatConnection();
  // }
}
