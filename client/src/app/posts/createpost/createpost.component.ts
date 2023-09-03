
import {Component, Inject} from '@angular/core';
import {MatDialog,MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { PostsService } from 'src/app/services/posts.service';
import { UsersService } from 'src/app/services/users.service';
import { Post } from '../../types/Post';


@Component({
  selector: 'app-createpost', 
  templateUrl: './createpost.component.html',
  styleUrls: ['./createpost.component.css']
})
export class CreatepostComponent   {
  constructor(public dialog: MatDialog) {}

  openDialog() {
    this.dialog.open(showing, {
      data: {title:'', message:'', selectedFile:''} as Post,
    });
  }
}

@Component({
  selector: 'showing',
  templateUrl: 'showing.html',
})

export class showing {
  base64Output : string ='';

  constructor(
    public dialogRef: MatDialogRef<CreatepostComponent>,
    @Inject(MAT_DIALOG_DATA) 
    public data: Post,
    private _snackBar: MatSnackBar,
    private postsService:PostsService,
    private usersService:UsersService ) {}
    
    CreatePost(){
      const {title, message, selectedFile} = this.data
      if(!title || !message || !selectedFile ){
       console.log('error')
       this.openSnackBar('Plese Complete The Form Failds')
      } else {
        // Creating the post
        this.postsService.createPost(this.data).subscribe({
          next:(res) =>{
            // after creating succeffly
            this.postsService.fetchPosts(1,this.usersService.UserServiceData._id as string);
            this.dialogRef.close();
            this.openSnackBar('Succesfully Creating the Post')
          },
          error:({error}) =>{
            this.openSnackBar(error.message)
          }
        })
       
      }
    }


    upload(event:any){
        var file = event.target.files.length;
        for(let i=0;i<file;i++)
        {
           var reader = new FileReader();
           reader.onload = (event:any) => 
           {
               this.data.selectedFile = event.target.result;
           }
           reader.readAsDataURL(event.target.files[i]);
        }
     }


    openSnackBar(message:string) {
       this._snackBar.open(message, 'Okay');
    }

}