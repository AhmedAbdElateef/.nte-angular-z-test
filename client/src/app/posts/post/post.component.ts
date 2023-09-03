import { Component, OnInit, Input} from '@angular/core';
import { Router } from '@angular/router';
import { PostsService } from 'src/app/services/posts.service';
import { UsersService } from 'src/app/services/users.service';
import { Post } from 'src/app/types/Post';
import { User } from 'src/app/types/User';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.css']
})
export class PostComponent implements OnInit {
   @Input() IsShowEdit:boolean =false;
   @Input() inProFile:boolean = false;
   @Input() post:Post  = {} as Post

   CreatorUser:User = {}

  // is in show edit post 
  // or not
  isLoved:boolean = false;
  Addcomment:string='';



  constructor( private router:Router, 
               private usersService:UsersService,
               private postsService:PostsService) {   }

  ngOnInit(): void {
    const { creator }= this.post;
    this.usersService.GetUserData(String(creator)).subscribe({
      next:({user})=>{
        this.CreatorUser = user;
        this.IsUserLiked()
       // console.log('userdata', user)
      }
    })
   console.log('post', this.post)
  }
  
  IsUserLiked(){
   this.isLoved = this.post.likes.includes(String(this.usersService.UserServiceData._id))
  }

  GoToDeatils(){
    this.router.navigate(['/EditShowPost' + `/${this.post?._id}`])
  }

  addCommentToPost(){
    this.postsService.comment(this.Addcomment, String(this.post?._id)).subscribe({
      next:({data})=>{
        console.log('commres', data.comments)
        this.post.comments = data.comments;
        this.Addcomment = ''
      }
    })
    console.log(this.Addcomment)
  }

  LovePost(){
    this.isLoved = !this.isLoved
    this.postsService.likePost(this.post._id).subscribe({
      next:({post})=>{
       console.log("post", post)
       this.post = post;
       this.IsUserLiked()
      }
    })
  }
}
