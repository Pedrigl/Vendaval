import { BaseModel } from "../shared/common/interfaces/BaseModel";
import { MessageMedia } from "./messagemedia";

export interface Message extends BaseModel{
  conversationId: number;
  senderId: string;
  receiverId: string;
  content: string;
  media: MessageMedia[];
}
