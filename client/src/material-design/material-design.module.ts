import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import {MatButtonModule} from '@angular/material/button';
import {MatIconModule} from '@angular/material/icon';
import {MatToolbarModule} from '@angular/material/toolbar';
import {MatMenuModule} from '@angular/material/menu';
import {MatInputModule} from '@angular/material/input';
import {MatGridListModule} from '@angular/material/grid-list';
import {MatCardModule} from '@angular/material/card';
import {MatListModule} from '@angular/material/list';
import {MatDialogModule} from '@angular/material/dialog';
import {MatSnackBarModule} from '@angular/material/snack-bar';
import {MatSlideToggleModule} from '@angular/material/slide-toggle';
import {MatPaginatorModule} from '@angular/material/paginator';



@NgModule({
  declarations: [],
  imports: [ CommonModule ],
  exports:[
    MatButtonModule,
    MatIconModule,
    MatToolbarModule,
    MatMenuModule,
    MatInputModule,
    MatGridListModule,
    MatCardModule,
    MatListModule,
    MatDialogModule,
    MatSnackBarModule,
    MatSlideToggleModule,
    MatPaginatorModule
  ]
})
export class MaterialDesignModule { }
