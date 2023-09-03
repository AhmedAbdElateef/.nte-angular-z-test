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



export class HomeGard implements CanActivate {
  constructor(public usersService: UsersService, public router: Router) {}
  // can activate
  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> | Promise<boolean> | boolean {
      if (this.usersService.isAuthenticated) { 
        console.log("auth.guard", this.usersService.isAuthenticated)
        // window.alert('Access Denied, This Page For NUN User Logeded in ');
         this.router.navigate(['/']);
       }
    return true;
  }
}


