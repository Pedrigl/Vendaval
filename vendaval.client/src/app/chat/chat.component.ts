import { Component, OnInit } from '@angular/core';
import { ChatService } from './chat.service';
import { BehaviorSubject, Observable, lastValueFrom, map } from 'rxjs';
import { ChatUser } from './chatuser';
import { Message } from './message';
import { LoadingService } from '../shared/common/loading.service';
import { AuthService } from '../shared/common/auth.service';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrl: './chat.component.css'
})
export class ChatComponent implements OnInit{
  user: BehaviorSubject<ChatUser | null> = new BehaviorSubject<ChatUser | null>( null);
  onlineSellers: ChatUser[] = [];
  onlineCustomers: BehaviorSubject<ChatUser[]> = new BehaviorSubject<ChatUser[]>([]);
  selectedUser: BehaviorSubject<ChatUser | null> = new BehaviorSubject<ChatUser | null>(null);
  messages: BehaviorSubject<Message[]> = new BehaviorSubject<Message[]>([]);
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
      this.chatService.startConnection().subscribe(() => {
        console.log('SignalR connection started');
      });

      this.chatService.getOwnChatUser().subscribe(user => {
        this.user.next(user);
      });

      this.chatService.getOnlineCustomers().subscribe(customers => {
        this.onlineCustomers.next(customers);

        if (customers.length > 0)
          setTimeout(() => {
            this.loadingService.isLoading.next(false);
          });
      })

      this.chatService.receiveMessage().subscribe(message => {
        this.messages.next([...this.messages.value, message]);
      });

    } catch (e:any) {
      console.log('Error initializing chat component: ', e.message);
    }
  }

  sendMessage(): void {

    this.selectedUser.subscribe(user => {
      console.log(user)
      console.log(this.user.value)
      if (user != null && this.user.value != null && this.text != '') {

        this.newMessage = {
          id: 1,
          senderId: this.user.value.connectionId,
          receiverId: user.connectionId,
          media: [],
          message: this.text,
          createdAt: new Date(),
          updatedAt: new Date()
        }

        this.chatService.sendMessage(this.newMessage);
        this.text = '';
      }
    });

    }
  }

