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
  onlineSellers: BehaviorSubject<ChatUser[]> = new BehaviorSubject<ChatUser[]>([]);
  onlineCustomers: BehaviorSubject<ChatUser[]> = new BehaviorSubject<ChatUser[]>([]);
  conversations: BehaviorSubject<[Conversation] | null> = new BehaviorSubject<[Conversation] | null>(null);
  text: string = '';
  newMessage!: Message;

  constructor(private chatService: ChatService, private authService: AuthService, private loadingService: LoadingService) {

  }
  async ngOnInit() {
    setTimeout(() => {
      this.loadingService.isLoading.next(true);
    });
    try {
      await this.chatService.initializeHubConnection();
      this.chatService.startConnection().subscribe();

      this.chatService.getOwnChatUser().subscribe(user => {
        this.chatUser.next(user);
      });

      this.chatService.getOnlineCustomers().subscribe(customers => {
        this.onlineCustomers.next(customers);

        if (customers.length > 0)
          setTimeout(() => {
            this.loadingService.isLoading.next(false);
          });
      })

      this.chatService.receiveMessage().subscribe(message => {
        var chatUser = this.onlineCustomers.value.find(x => x.connectionId == message.senderId);

      });



    } catch (e: any) {
      console.log('Error initializing chat component: ', e.message);
    }
  }
}
  
