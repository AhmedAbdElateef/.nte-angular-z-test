import { Component, OnInit } from '@angular/core';
import { User } from '../types/User';
import { MatSnackBar } from '@angular/material/snack-bar';
import { UsersService } from '../services/users.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.css']
})
export class AuthComponent implements OnInit {
  loadLogin:boolean = false;
  loadSiginup:boolean = false;

  loginForm: User = {
    email: '',
    password: '',
  };
  
  SiginUpForm = {
    email:'',
    password:'',
    firstName:'',
    lastName:'',
  }

  constructor(
              private _snackBar: MatSnackBar, 
              private usersService:UsersService,
              private router:Router) { }

  ngOnInit(): void {
  }

  Login() {
    this.loadLogin = !this.loadLogin
    const {email, password} = this.loginForm;

    if(email=='' || password == ''){
     this._snackBar
         .open('Please Compete The Form Correctlly', 'Try Again' )
         .onAction().subscribe(()=> this.loadLogin = !this.loadLogin)    
    } else {
      // Start Login
      this.usersService.signIn(this.loginForm).subscribe({ 
        next: async (res) => {
          if(res){
            this.loadLogin = !this.loadLogin
            this.usersService.isAuthenticated = true;
            this.usersService.CheckIfUserIsAuthenticated.emit(true);
            this.router.navigate(['/profile/', res.result._id])
          }
        },
        error: ({error}) => {
          this.loadLogin = !this.loadLogin
          this._snackBar.open(error.message, 'Try Again' )
        },
      })
    }
  }

  Register(){
    this.loadSiginup = !this.loadSiginup
    const {email, password, firstName, lastName} = this.SiginUpForm;

    if(email=='' || password == '' || firstName =='' || lastName==''){
     this._snackBar
         .open('Please Compete The Form Correctlly', 'Try Again' )
         .onAction().subscribe(()=> this.loadSiginup = !this.loadSiginup)    
    } else {
      // Start Login
      this.usersService.signUp(this.SiginUpForm).subscribe({
        next: (res) => {
          this.usersService.UserServiceData = res.result;
          localStorage.setItem('profile', JSON.stringify({ ...res }));
          this.router.navigate(['/profile/', res.result._id])
          this.loadSiginup = !this.loadSiginup
          this.usersService.isAuthenticated = true;
          this.usersService.CheckIfUserIsAuthenticated.emit(true);
          this.usersService.isUserAuth();
          // this.usersService.isAuthenticated = true;

        },
        error: ({error}) => {
          this.loadSiginup = !this.loadSiginup
          this._snackBar.open(error.message, 'Try Again' )
        }
      })
      // console.log(this.SiginUpForm)
    }
  }

  

}
