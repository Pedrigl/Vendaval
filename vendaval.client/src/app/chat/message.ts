export interface Message {
  id: Number;
  senderId: String;
  receiverId: String;
  message: string;
  media: string[];
  createdAt: Date;
  updatedAt: Date;
}
