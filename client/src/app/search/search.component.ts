import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PostsService } from '../services/posts.service';
import { Post } from '../types/Post';
import { User } from '../types/User';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit {
  Search_Data:string =''
  UserOrPost:boolean = false;

  Users:User[] = []

  Posts:Post[] = []


  constructor(  
    private route: ActivatedRoute,
    private postsService:PostsService
    ) { }

  ngOnInit() {
    this.route.queryParamMap.subscribe(params => 
      {
        this.Search_Data = params?.get('data') as string;
        this.postsService.fetchPostsUsersBySearch(this.Search_Data)
        .subscribe((data)=>{
          this.Posts = data?.posts;
          this.Users = data?.user;
        })
      }
    )

 
  }

  changeSearch(){
    this.UserOrPost = !this.UserOrPost
  }
}
