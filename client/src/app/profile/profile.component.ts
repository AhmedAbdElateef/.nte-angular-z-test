import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UsersService } from '../services/users.service';
import { Post } from '../types/Post';
import { User } from '../types/User';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  posts:Array<Post> = []
  
  userData:User = {} as User
  
  edit:boolean = false

  isSameUser:boolean = false

  isFollow: boolean = false


  constructor(private route: ActivatedRoute, private usersService:UsersService) { }

  ngOnInit(): void {
    const LogdUser =  this.usersService.UserServiceData._id;
    this.usersService.GetUserData(this.route.snapshot.params['id']).subscribe({
      next:(res)=>{
         console.log(res)
        this.posts = res.posts;
        this.userData = res.user

        if(res.user._id == LogdUser) {   this.isSameUser = true }
        if( res.user.followers.find((id:any) => id ===  this.usersService.UserServiceData._id) ) {   
           this.isFollow = true
        }
      }
    })
  }

  Updated(){
   this.edit = !this.edit
  }

  follow(){
    this.isFollow = !this.isFollow

    this.usersService.following(String(this.userData._id)).subscribe({
      next:()=>{
      }
    })
    console.log('follow')
  }

}
