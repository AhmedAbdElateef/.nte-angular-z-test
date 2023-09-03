import { Injectable } from '@angular/core';
import {Observable, empty} from 'rxjs'
import {HttpClient, HttpHeaders} from '@angular/common/http'

import { Post } from '../types/Post';
import { environment } from 'src/environments/environment';

function httpOptions(){

  var opt = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${JSON.parse(localStorage.getItem('profile') as string)?.token}`
    })
  }
  return opt;
}

@Injectable({
  providedIn: 'root'
})
export class PostsService {
  userid : string =  JSON.parse(localStorage.getItem('profile') as string)?.result?._id;

  private apiUrl = environment.APIUrl+'posts'

  PostsServiceData:Array<any>=[]

  
  constructor(private http:HttpClient) { }

  // get one post by id Start //
  fetchPost(id:string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}`)
  }
  // get one post by id End

  // fetch Posts Users By Search Start
  fetchPostsUsersBySearch(searchQuery:string): Observable<{user:[], posts:[]}> {
    return this.http.get<{user:[], posts:[]}>(`${this.apiUrl}/search?searchQuery=${searchQuery}`)
  }
  // fetch Posts Users By Search End

  // fetch multi posts start 
  fetchPosts(page:number, id:string): Observable<any> {
    if(this.userid){
      const posts = this.http.get<any>(`${this.apiUrl}?page=${page}&id=${id}`)
      
      posts.subscribe({
        next:({ data })=>{ 
          // first clear old array 
            this.PostsServiceData = [];
          for (let i = 0; i <  data.length; i++) {
            const el =  data[i];
            this.PostsServiceData.push(el)
          }
        }
      })

      return posts;
    } else {
      return empty();
    }

  }
  // fetch multi posts end

  // create post start 
  createPost(newPost:Post): Observable<Post>{
    return this.http.post<Post>(`${this.apiUrl}`, newPost , httpOptions() )
     }
  // create post end

  // comment post start 
  comment(value:string, id:string): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/${id}/commentPost`, { value },  httpOptions() )
  }
  // comment post end


  // like post start
  likePost(id:any): Observable<any>{
    
    //console.log(id)
    return this.http.patch<any>(`${this.apiUrl}/${id}/likePost`,{} , httpOptions() )
  }
  // like post end


  // update post start 
  updatePost(id:string, updatedPost:Post): Observable<Post>{
    return this.http.patch<Post>(`${this.apiUrl}/${id}`, updatedPost, httpOptions() )
  }
  // update post end

  // delete post start 
  deletePost(id:string) :Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${id}`,  httpOptions() )
  }
  // delete post end

}
