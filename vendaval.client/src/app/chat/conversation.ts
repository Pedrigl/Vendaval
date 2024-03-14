import { BaseModel } from "../shared/common/interfaces/BaseModel";
import { ChatUser } from "./chatuser";
import { Message } from "./message";

export interface Conversation extends BaseModel{
  participants: ChatUser[];
  messages: Message[];
}
