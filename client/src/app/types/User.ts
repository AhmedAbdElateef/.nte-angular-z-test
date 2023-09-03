export interface User {
    _id?:string,
    name?: string,
    email?: string,
    password?: string,
    id?:string,
    imageUrl?:string, 
    bio?:string,
    followers?:Array<any>,
    following?:Array<any>,
  }



export interface UserInChat {
    id?:string,
    _id?:string,
    name?: string,
    imageUrl?:string, 
    unReadedMessage: number
  }





