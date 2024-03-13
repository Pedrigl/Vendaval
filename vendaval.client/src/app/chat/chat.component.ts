import { Component, OnInit } from '@angular/core';
import { ChatService } from './chat.service';
import { lastValueFrom, map } from 'rxjs';
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
  user!: ChatUser;
  onlineSellers: ChatUser[] = [];
  onlineCustomers: ChatUser[] = [];
  selectedUser!: ChatUser;
  messages: Message[] = [];
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
      await this.chatService.ownChatUser$.subscribe(user => {
        if(user != null)
          this.user = user;
      });
      this.chatService.getOnlineCustomers();
      this.chatService.onlineCustomers$.subscribe(customers => {
      this.onlineCustomers = customers;

        if(customers.length > 0)
          setTimeout(() => {
            this.loadingService.isLoading.next(false);
          });

        this.chatService.messages$.subscribe(messages => {
          console.log("my connectionId: ", this.user.connectionId);
          this.messages = messages;
        });
      })
      
    } catch (e:any) {
      console.log('Error initializing chat component: ', e.message);
    }
  }

  sendMessage(): void {
    if (this.selectedUser != null && this.text) {
      this.newMessage = {
        Id: 1,
        SenderId: this.user.connectionId,
        ReceiverId: this.selectedUser.connectionId,
        Media: [],
        Message: this.text,
        CreatedAt: new Date(),
        UpdatedAt: new Date()
      }

      this.chatService.sendMessage(this.newMessage);
      this.text = '';
    }
  }

}
