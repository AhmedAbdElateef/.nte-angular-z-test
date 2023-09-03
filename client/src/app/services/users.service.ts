import { EventEmitter, Injectable } from '@angular/core';
import {Observable} from 'rxjs'
import {HttpClient, HttpHeaders} from '@angular/common/http'

import decode from 'jwt-decode';

import { User } from '../types/User';
import { Router } from '@angular/router';
import { Post } from '../types/Post';
import { environment } from 'src/environments/environment';
import { RealtimechatService } from './realtimechat.service';

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
export class UsersService {
  // isAuthenticated: boolean = false;
  UserServiceData:User = {};
  private apiUrl = environment.APIUrl+'user'
  userid : string =  JSON.parse(localStorage.getItem('profile') as string)?.result?._id;

  CheckIfUserIsAuthenticated: EventEmitter<boolean> = new EventEmitter<boolean>();
  isAuthenticated: boolean = false;

  constructor(private http:HttpClient, private router:Router, private realtimeService: RealtimechatService) { }
  // update auth
  UpdateAuth(){
    if(this.userid){
      // this.CheckIfUserIsAuthenticated.emit(true);
      this.isAuthenticated = true;
      this.isUserAuth();
    } else {
      // this.CheckIfUserIsAuthenticated.emit(false);
      this.isAuthenticated = false;
    }
    // this.CheckIfUserIsAuthenticated.subscribe((isAuht)=>{
    //   if(isAuht){
    //     this.CheckIfUserIsAuthenticated.emit(true);
    //     this.isAuthenticated = true;
    //     this.isUserAuth(); 
    //   } else {
    //     this.CheckIfUserIsAuthenticated.emit(false);
    //     this.isAuthenticated = false;
    //   }
    // });
  }

  // end update auth


  // LogIn User start //
  signIn(formData:User): Observable<any> {
    let auth = this.http.post<User>(`${this.apiUrl}/signin`, formData)
    auth.subscribe({
      next: (res:any) => {
        // this.isAuthenticated = true;
        this.UserServiceData = res.result;
        localStorage.setItem('profile', JSON.stringify({ ...res }));
      }
    })

    return auth
  }
  // logIn User End //

  // Sign Up User start //
  signUp(formData:User): Observable<any> {
    const d =  this.http.post<User>(`${this.apiUrl}/signup`, formData)
    d.subscribe({
      next:(r)=>{
        // this.realtimeService.StartUserConnection();
        // this.realtimeService.createChatConnection();
      }
    })
    return d;
  }
  // Sign Up User End //

  // Get User data & posts Start //
  GetUserData(id:string): Observable<Post&User|any> {
    return this.http.get<Post&User|any>(`${this.apiUrl}/getUser/${id}`)
  }
  // Get User data & posts end //

  // Get Suggestion Users start //
  getSugUser(id:string): Observable<User[] | any > {
    return this.http.get<User[]>(`${this.apiUrl}/getSug?id=${id}`, httpOptions())
  }
  // Get Suggestion Users End //

  // Update User Data Start //
  UpdateUser(userData:User) : Observable<User> {

    return this.http
           .patch<User>(`${this.apiUrl}/Update/${userData._id}`, userData, httpOptions())
  }
  // Update User Data End //

  // Following Start //
  following(id:string) : Observable<User> {
    return this.http.patch<User>(`${this.apiUrl}/${id}/following`,{}, httpOptions())
  }
  // Following End //


  // Check is User is Authenticated start //
  async isUserAuth(){
    const user = JSON.parse(localStorage.getItem('profile') as string)
    const token = user?.token;
    
    // update user data 
    if (user){
      var up = this.GetUserData(user?.result?._id);
      up.subscribe({
        next: (res:any) => {
          var u = user?.result;
          u.following = res?.user?.following
          u.followers = res?.user?.followers
          u.name = res?.user?.name
          u.bio = res?.user?.bio
          u.imageUrl = res?.user?.imageUrl
          user.result = u;
          localStorage.setItem('profile', JSON.stringify({ ...user }));
          //  
          // this.CheckIfUserIsAuthenticated.emit(true);
        }
      })
    }

    // 
    if (token) {
      const decodedToken:any = decode(token);
      if (decodedToken.exp * 1000 < new Date().getTime()) {
       this.LogOut()
     } else {
       this.UserServiceData = user.result;
      //  this.isAuthenticated = true
     }
   }
  }
  // check is User is Authenticated End //
  
  // LogOut start //
  LogOut(){
    localStorage.clear();
    // this.isAuthenticated = false;
    this.realtimeService.stopChatConnection();
    this.CheckIfUserIsAuthenticated.emit(false);
    this.isAuthenticated =false;
    this.UserServiceData = {};
    this.router.navigate(['/auth'])
  }
  // LogOut End //
}
