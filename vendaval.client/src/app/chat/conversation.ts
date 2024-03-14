import { ChatUser } from "./chatuser";
import { Message } from "./message";

export interface Conversation {
  sender: ChatUser;
  messages: Message[];
}
