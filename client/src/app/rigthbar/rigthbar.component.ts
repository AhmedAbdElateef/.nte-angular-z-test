import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UsersService } from '../services/users.service';

@Component({
  selector: 'app-rigthbar',
  templateUrl: './rigthbar.component.html',
  styleUrls: ['./rigthbar.component.css']
})
export class RigthbarComponent implements OnInit {

  sugUsersList:any = []
  constructor(private userService:UsersService, private route:Router) { }

  ngOnInit(): void {

    if(this.userService.isAuthenticated && this.userService.UserServiceData?._id != null){
      this.userService.getSugUser(String(this.userService.UserServiceData?._id)).subscribe({
        next:({users})=>{
          console.log(users);
          this.sugUsersList = users
        }
       })
     }    



  }

  MoveToUserPage(id:any){
    this.route.navigate(['/profile/', id])
  }

}
