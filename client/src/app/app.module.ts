import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialDesignModule } from '../material-design/material-design.module';
import { HomeComponent } from './home/home.component';
import { NavbarComponent } from './navbar/navbar.component';
import { PostComponent } from './posts/post/post.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { RigthbarComponent } from './rigthbar/rigthbar.component';
import { CreatepostComponent, showing } from './posts/createpost/createpost.component';
import { AuthComponent } from './auth/auth.component';
import { SearchComponent } from './search/search.component';
import { ProfileComponent } from './profile/profile.component';
import { EditshwopostComponent } from './posts/editshwopost/editshwopost.component';
import { UpdateComponent } from './profile/update/update.component';
import { HttpClientModule } from '@angular/common/http';
import { ChatComponent } from './chat/chat.component';
import { NotificationComponent } from './notification/notification.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    NavbarComponent,
    PostComponent,
    SidebarComponent,
    RigthbarComponent,
    CreatepostComponent,
    showing,
    AuthComponent,
    SearchComponent,
    ProfileComponent,
    EditshwopostComponent,
    UpdateComponent,
    ChatComponent,
    NotificationComponent
    
  ],
  imports: [
    FormsModule,
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MaterialDesignModule,
    
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
