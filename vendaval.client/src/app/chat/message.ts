export interface Message {
  id: Number;
  senderId: string;
  receiverId: string;
  message: string;
  media: string[];
  createdAt: Date;
  updatedAt: Date;
}
