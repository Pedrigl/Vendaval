import { Component, OnInit } from '@angular/core';
import { ChatService } from './chat.service';
import { BehaviorSubject, Observable, lastValueFrom, map } from 'rxjs';
import { ChatUser } from './chatuser';
import { Message } from './message';
import { LoadingService } from '../shared/common/loading.service';
import { AuthService } from '../shared/common/auth.service';
import { KeyValue } from '@angular/common';
import { Conversation } from './conversation';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrl: './chat.component.css'
})
export class ChatComponent implements OnInit {
  chatUser: BehaviorSubject<ChatUser | null> = new BehaviorSubject<ChatUser | null>(null);
  onlineSellers: BehaviorSubject<ChatUser[]>;
  onlineCustomers: BehaviorSubject<ChatUser[]>;
  conversations: BehaviorSubject<Conversation[]>;
  selectedConversation: BehaviorSubject<Conversation | null> = new BehaviorSubject<Conversation | null>(null);
  text: string = '';
  newMessage!: Message;

  constructor(private chatService: ChatService, private authService: AuthService) {
    this.onlineSellers = new BehaviorSubject<ChatUser[]>([]);
    this.onlineCustomers = new BehaviorSubject<ChatUser[]>([]);
    this.conversations = new BehaviorSubject<Conversation[]>([]);
  }
  async ngOnInit() {
    
    try {
      await this.chatService.initializeHubConnection();
      this.chatService.startConnection().subscribe();

      this.chatService.getOwnChatUser().subscribe(user => {
        this.chatUser.next(user);
      });

      this.chatService.getOnlineCustomers().subscribe(customers => {
        this.onlineCustomers.next(customers);
      })

      this.chatService.getOnlineSellers().subscribe(sellers => {
        this.onlineSellers.next(sellers);
      })

      this.chatService.receiveMessage().subscribe(message => {
        var chatUser = this.onlineCustomers.value.find(x => x.connectionId == message.senderId);
      });



    } catch (e: any) {
      console.log('Error initializing chat component: ', e.message);
    }
  }

  getOppositeUser(conversationUsers : ChatUser[]): ChatUser | undefined {
    return conversationUsers.find(u => u.connectionId != this.chatUser.value?.connectionId);
  }

  sendMessage() {

  }

  selectConversation(conversation: Conversation) {
    this.selectedConversation.next(conversation);
  }


}
  
