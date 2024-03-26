import { Component, OnInit } from '@angular/core';
import { ChatService } from '../chat.service';
import { BehaviorSubject } from 'rxjs';
import { Conversation } from '../conversation';
import { ChatUser } from '../chatuser';

@Component({
  selector: 'app-chat-costumer',
  templateUrl: './chat-costumer.component.html',
  styleUrl: './chat-costumer.component.css'
})
export class ChatCostumerComponent implements OnInit{
  conversation: BehaviorSubject<Conversation | null>;
  chatUser: BehaviorSubject<ChatUser | null>; 
  onlineSellers: BehaviorSubject<ChatUser[]>;
  text: string;
  constructor(private chatService: ChatService) {
    this.conversation = new BehaviorSubject<Conversation | null>(null);
    this.chatUser = new BehaviorSubject<ChatUser | null>(null);
    this.onlineSellers = new BehaviorSubject<ChatUser[]>([]);
    this.text = '';
  }

  async ngOnInit() {
    try {
      await this.chatService.initializeHubConnection();
      this.chatService.startConnection().subscribe();

      this.chatService.getOwnChatUser().subscribe(user => {
        this.chatUser.next(user);
      });

    }
    catch (e: any) {
      console.log('Error initializing chat component: ', e.message);
    }
  }
}
