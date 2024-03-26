import { Component, OnInit } from '@angular/core';
import { ChatService } from '../chat.service';
import { BehaviorSubject, Observable, lastValueFrom, map } from 'rxjs';
import { ChatUser } from '../chatuser';
import { Message } from '../message';
import { LoadingService } from '../../shared/common/loading.service';
import { AuthService } from '../../shared/common/auth.service';
import { KeyValue } from '@angular/common';
import { Conversation } from '../conversation';

@Component({
  selector: 'app-chat-seller',
  templateUrl: './chat-seller.component.html',
  styleUrl: './chat-seller.component.css'
})
export class ChatSellerComponent implements OnInit {
  chatUser: BehaviorSubject<ChatUser | null> = new BehaviorSubject<ChatUser | null>(null);
  onlineCustomers: BehaviorSubject<ChatUser[]>;
  conversations: BehaviorSubject<Conversation[]>;
  selectedConversation: BehaviorSubject<Conversation | null> = new BehaviorSubject<Conversation | null>(null);
  text: string = '';
  newMessage!: Message;

  constructor(private chatService: ChatService, private authService: AuthService) {
    this.onlineCustomers = new BehaviorSubject<ChatUser[]>([]);
    this.conversations = new BehaviorSubject<Conversation[]>([]);
    this.newMessage = {
      id: 0,
      media: [],
      createdAt: new Date(),
      updatedAt: new Date(),
      content: '',
      receiverId: 0,
      senderId: 0,
      senderConnectionId: this.chatUser.value?.connectionId || '',
      receiverConnectionId: '',
      conversationId: 0
    }
  }
  async ngOnInit() {
    
    try {
      LoadingService.isLoading.next(true);
      await this.chatService.initializeHubConnection();
      this.chatService.startConnection().subscribe();

      this.chatService.getOwnChatUser().subscribe(user => {
        this.chatUser.next(user);
      });

      this.chatService.getOnlineCustomers().subscribe(customers => {
        LoadingService.isLoading.next(false);
        this.onlineCustomers.next(customers);

        this.updateSelectedConversationConnectionId(customers);
      })

      this.chatService.receiveUserConversations().subscribe(conversations => {
        this.conversations.next(conversations);
      })
      this.chatService.receiveMessage().subscribe(message => {
        this.conversations.value.filter(c => c.id == message.conversationId)[0].messages.push(message);
      });



    } catch (e: any) {
      console.log('Error initializing chat component: ', e.message);
    }
  }

  private updateSelectedConversationConnectionId(customers: ChatUser[]) {
      if (this.selectedConversation.value) {
          const oppositeUser = this.getOppositeUser(this.selectedConversation.value.participants);
          if (oppositeUser) {
              const updatedUser = customers.find(c => c.id === oppositeUser.id);
              if (updatedUser) {
                  const index = this.selectedConversation.value.participants.indexOf(oppositeUser);
                  if (index !== -1) {
                      this.selectedConversation.value.participants[index] = updatedUser;
                      this.selectedConversation.next(this.selectedConversation.value);
                  }
              }
          }
      }
  }

  getMessageSenderName(message: Message): string {
    return this.selectedConversation.value?.participants.find(u => u.id == message.senderId)?.name || '';
  }
  getOppositeUser(conversationUsers : ChatUser[]): ChatUser | undefined {
    return conversationUsers.find(u => u.connectionId != this.chatUser.value?.connectionId);
  }

  sendMessage() {
    var users = this.selectedConversation.value?.participants;
    if(users == null)
      return;
    
    this.newMessage.content = this.text;
    
    this.chatService.sendMessage(this.newMessage, users);
    this.text = '';
  }

  startConversation(user: ChatUser) {
    var ownUser = this.chatUser.value;
    if(ownUser == null)
      return;
    
    this.chatService.createConversation([ownUser,user]);
  }

  selectConversation(conversation: Conversation) {
    this.newMessage.conversationId = conversation.id;
    this.newMessage.senderId = this.chatUser.value?.id || 0;
    this.newMessage.senderConnectionId = this.chatUser.value?.connectionId || '';
    this.newMessage.receiverId = this.getOppositeUser(conversation.participants)?.id || 0;
    this.newMessage.receiverConnectionId = this.getOppositeUser(conversation.participants)?.connectionId || '';
    this.newMessage.content = this.text;

    this.selectedConversation.next(conversation);
  }

  deleteConversation(conversation: Conversation) {
    if (this.selectedConversation.value?.id == conversation.id)
      this.selectedConversation.next(null);

    this.chatService.deleteConversation(conversation);
  }
}
  
