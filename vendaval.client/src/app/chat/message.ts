export interface Message {
  Id: Number;
  SenderId: String;
  ReceiverId: String;
  Message: string;
  Media: string[];
  CreatedAt: Date;
  UpdatedAt: Date;
}
