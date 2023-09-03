import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthComponent } from './auth/auth.component';
import {  HomeGard } from './auth/auth.guard';
import { HomeComponent } from './home/home.component';
import { AuthGuard } from './home/home.guard';
import { EditshwopostComponent } from './posts/editshwopost/editshwopost.component';
import { ProfileComponent } from './profile/profile.component';
import { SearchComponent } from './search/search.component';
import { ChatComponent } from './chat/chat.component';
import { NotificationComponent } from './notification/notification.component';

const routes: Routes = [
  {path:'', component:HomeComponent},
  {path:'auth', component:AuthComponent, canActivate:[HomeGard]},
  {path:'search', component:SearchComponent},
  {path:'chat', component:ChatComponent,  canActivate:[AuthGuard]},
  {path:'notifications', component:NotificationComponent},
  {path:'profile/:id', component:ProfileComponent, canActivate:[AuthGuard]},
  {path:'EditShowPost/:id', component:EditshwopostComponent , canActivate:[AuthGuard]}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
