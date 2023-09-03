export interface Post {
    _id?:string,
    title?: string,
    message?: string,
    name?: string,
    creator?: object,
    selectedFile?: string,
    likes: Array<string>,
    comments:Array<any>,
    createdAt?:  Date,
  }