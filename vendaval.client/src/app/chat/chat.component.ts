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
export class ChatComponent implements OnInit{
  user: BehaviorSubject<ChatUser | null> = new BehaviorSubject<ChatUser | null>( null);
  onlineSellers: BehaviorSubject<ChatUser[]> = new BehaviorSubject<ChatUser[]>([]);
  onlineCustomers: BehaviorSubject<ChatUser[]> = new BehaviorSubject<ChatUser[]>([]);
  selectedUser: BehaviorSubject<ChatUser | null> = new BehaviorSubject<ChatUser | null>(null);
  messages: BehaviorSubject<Message[]> = new BehaviorSubject<Message[]>([]);
  conversations: BehaviorSubject<[Conversation] | null>  = new BehaviorSubject<[Conversation] | null>(null);
  text: string = '';
  newMessage!: Message;

  constructor(private chatService: ChatService, private authService: AuthService, private loadingService: LoadingService) {
    //TODO: make this code more readable, less complex and more maintainable
    //TODO: place a notification bubble on the chat icon when a new message is received
    //TODO: fix bug where if you select a user after it has sent a message, it will display messages not sent for the selected conversation
  }
  async ngOnInit() {
    setTimeout(() => {
      this.loadingService.isLoading.next(true);
    });
    try {
      await this.chatService.initializeHubConnection();
      this.chatService.startConnection().subscribe();

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
        var chatUser = this.onlineCustomers.value.find(x => x.connectionId == message.senderId);

        if (chatUser != null) {
          if (!this.conversations.value) {
            this.conversations.next([{ sender: chatUser, messages: [] }]);
          }

          var chatUserConversation = this.conversations.value?.find(c => c.sender == chatUser)
          console.log(chatUserConversation);

          if (chatUserConversation == null) {
            var conversation: Conversation = {
              sender: chatUser,
              messages: [message]
            }
            chatUserConversation = conversation;
          }
          else {
            chatUserConversation.messages.push(message);
          }

          var conversations = this.conversations.value;
          conversations?.push(chatUserConversation);

          this.conversations.next(conversations);

          if (this.selectedUser.value && (this.selectedUser.value.connectionId === message.senderId ||this.selectedUser.value.connectionId == message.receiverId)) {
            this.messages.next([...this.messages.value, message]);
          }
        }
      });



    } catch (e:any) {
      console.log('Error initializing chat component: ', e.message);
    }
  }

  getUserName(senderId: string): string {
    let user = this.onlineCustomers.value.find(x => x.connectionId === senderId) || this.onlineSellers.value.find(x => x.connectionId === senderId);
    return user ? user.name : '';
  }

  selectUser(user: ChatUser) {
    this.selectedUser.next(user);
    var chatUserConversation = this.conversations.value?.find(c => c.sender == user)
    if (chatUserConversation != null) {
      this.messages.next(chatUserConversation.messages);
    }
  }
  sendMessage(): void {

    this.selectedUser.subscribe(user => {
      
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

