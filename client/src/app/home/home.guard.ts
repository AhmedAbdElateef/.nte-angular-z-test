
import { Injectable } from '@angular/core';
import {
  CanActivate,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  Router,
} from '@angular/router';
import { UsersService } from  '../services/users.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})



export class AuthGuard implements CanActivate {
  constructor(public usersService: UsersService, public router: Router) {}
  // can activate
  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> | Promise<boolean> | boolean {
    // this.usersService.CheckIfUserIsAuthenticated.emit(false);
    // this.usersService.CheckIfUserIsAuthenticated.subscribe((isauth)=>{
      // console.log("homeGuard Auth", isauth)
      if (!this.usersService.isAuthenticated) {
        //window.alert('Access Denied, Login is Required to Access This Page!');
        this.router.navigate(['auth']);
      }
    // })

    return true;
  }
}