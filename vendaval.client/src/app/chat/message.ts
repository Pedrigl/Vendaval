import { BaseModel } from "../shared/common/interfaces/BaseModel";
import { MessageMedia } from "./messagemedia";

export interface Message extends BaseModel{
  conversationId: number;
  senderId: number;
  receiverId: number;
  senderConnectionId: string;
  receiverConnectionId: string;
  content: string;
  media: MessageMedia[];
}
