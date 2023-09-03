import { Component, OnInit } from '@angular/core';
import { UsersService } from '../services/users.service';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {

  userid:string = '';

  constructor(public usersService: UsersService) { }

  ngOnInit(): void {
    

    // this.usersService.CheckIfUserIsAuthenticated.subscribe((isauth)=>{
    // })
    
    const user = JSON.parse(localStorage.getItem('profile') as string)

    if( user ) this.userid = user.result._id
    
  }

}
