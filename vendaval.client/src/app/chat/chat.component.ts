import { Component, OnInit } from '@angular/core';
import { ChatService } from './chat.service';
import { map } from 'rxjs';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrl: './chat.component.css'
})
export class ChatComponent implements OnInit{
  onlineSellers: string[] = [];
  onlineCustomers: string[] = [];
  messages: string[] = [];
  newMessage!: string;

  constructor(private chatService: ChatService) {

  }

  async ngOnInit() {
    try {
      await this.chatService.initializeHubConnection();
      this.chatService.getOnlineCustomers();
      this.chatService.onlineCustomers$.subscribe(customers => {
        this.onlineCustomers = customers;
      })
      
    } catch (e:any) {
      console.log('Error initializing chat component: ', e.message);
    }
  }

  sendMessage(): void {
    this.chatService.sendMessage(this.newMessage);
    this.newMessage = '';
  }

}
