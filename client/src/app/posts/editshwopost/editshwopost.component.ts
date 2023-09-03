import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PostsService } from 'src/app/services/posts.service';
import { UsersService } from 'src/app/services/users.service';
import { Post } from 'src/app/types/Post';

@Component({
  selector: 'app-editshwopost',
  templateUrl: './editshwopost.component.html',
  styleUrls: ['./editshwopost.component.css']
})
export class EditshwopostComponent implements OnInit {
  IS_Edit:boolean = false;
  post:Post = {} as Post;

  If_UserIsTheCeator = false;

  constructor(
              private postsService:PostsService,
              private route: ActivatedRoute,
              private userS:UsersService ,
              private router:Router) { }
  
  ngOnInit() {
   this.postsService.fetchPost(this.route.snapshot.params['id']).subscribe({
    next:({post})=>{
      console.log("pooost", post)
      this.post = post;
      // checkif the user is the creator
      if(this.userS.UserServiceData._id === post.creator){
        this.If_UserIsTheCeator = true
      }
    }
   })
 
  }

  upload(event:any){
    var file = event.target.files.length;
    for(let i=0;i<file;i++)
    {
       var reader = new FileReader();
       reader.onload = (event:any) => 
       {
           this.post.selectedFile = event.target.result;
       }
       reader.readAsDataURL(event.target.files[i]);
    }
 }

 UpdateAndSave(){
  this.IS_Edit = false
  
  if(this.If_UserIsTheCeator){
    
    this.postsService.updatePost(String(this.post._id),this.post).subscribe({
      next:(res)=>{
        console.log('up post', res)
      }
    })
  }
 }

 Delete(){
  if(this.If_UserIsTheCeator){
    
    this.postsService.deletePost(String(this.post._id)).subscribe({
      next:(res)=>{
        console.log('up post', res)
        this.router.navigate(['/auth'])
      }
    })
  }
 }

}
