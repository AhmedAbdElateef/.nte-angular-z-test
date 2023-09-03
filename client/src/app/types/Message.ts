export interface Message {
    _id?:string,
    content?: string,
    sender?: string,
    recever?: string,
  }



export interface  UnrededMsg {

  _id?:string,
  mainUserid?:string,
  otherUserid?:string,
  numOfUneadedMessages:Number,
  isReaded?:boolean
}


