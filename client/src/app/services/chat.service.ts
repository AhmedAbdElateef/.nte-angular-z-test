import { Injectable } from '@angular/core';
import {Observable} from 'rxjs'
import {HttpClient, HttpHeaders} from '@angular/common/http'


import { Message, UnrededMsg } from '../types/Message';
import { Router } from '@angular/router';
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


export class ChatService {

  private apiUrl = environment.APIUrl+'chat'
  public UnreadedMsgNum : Number = 0;
  public UnreadedMsgList : UnrededMsg[] = [];
  userid : string =  JSON.parse(localStorage.getItem('profile') as string)?.result?._id;

  constructor(private http:HttpClient, private router:Router) { }

  sendMessage(formData:Message): Observable<any> {
    return this.http.post<Message>(`${this.apiUrl}/sendmessage`, formData)
  }

  getMessagesBetweenTwoUsers(from:any, firstuid: string, seconduid: string): Observable<{ msgs: Message[] }> {
    return this.http.get<{ msgs: Message[] }>(`${this.apiUrl}/getmsgsbynums?from=${from}&firstuid=${firstuid}&seconduid=${seconduid}`)
  }


  getUnreadedMessagesNum() : Observable<any> {
    var data= this.http.get<{total : Number, messages: UnrededMsg[]}>(`${this.apiUrl}/get-user-unraededmsg?userid=${this.userid}`);
    data.subscribe({
      next:({ total, messages })=>{ 
        this.UnreadedMsgNum = total;
        this.UnreadedMsgList = messages;
        console.log("service data here urm", this.UnreadedMsgList)
      }
    })
    return data;
  }



  async MarkmsgAsReaded(fuid: string, suid: string) {
    console.log("markmsg called")
    var data =  await this.http.get<any>(`${this.apiUrl}/mark-msg-asreaded?mainuid=${fuid}&otheruid=${suid}`)
    data.subscribe({
      next:(res)=>{
        // console.log("res mark", res)
        if(res?.isMarked){
          this.getUnreadedMessagesNum();
        }
      }
    })
  }

}
