import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { UsersService } from 'src/app/services/users.service';
import { User } from 'src/app/types/User';

@Component({
  selector: 'app-update',
  templateUrl: './update.component.html',
  styleUrls: ['./update.component.css']
})
export class UpdateComponent implements OnInit {
  @Output() ChageUpdate: EventEmitter<boolean> = new EventEmitter();
  @Input() user:User  = {} as User

  constructor(private usersService:UsersService) { }

  ngOnInit(): void {
  }

  upload(event:any){
    var file = event.target.files.length;
    for(let i=0;i<file;i++)
    {
       var reader = new FileReader();
       reader.onload = (event:any) => 
       {
           this.user.imageUrl = event.target.result;
       }
       reader.readAsDataURL(event.target.files[i]);
    }
 }

  Update(){
    this.usersService.UpdateUser(this.user).subscribe({
      next:()=>{
        this.ChageUpdate.emit();
      }
    })
  }
}
