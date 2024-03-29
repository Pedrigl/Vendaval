import { Injectable, OnInit } from '@angular/core';
import { BehaviorSubject, Observable, lastValueFrom, map, throwError } from 'rxjs';
import * as SignalR from '@microsoft/signalr';
import { environment } from '../../environments/environment';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../shared/common/auth.service';
import { AuthorizedHttpClient } from '../shared/common/authorized-httpclient';
import { ChatUser } from './chatuser';
import { Message } from './message';
import { Conversation } from './conversation';

@Injectable({
  providedIn: 'root'
})
export class ChatService { 
  private hubConnection!: HubConnection;
  private hubUrl = environment.apiUrl + "chathub";
  constructor(private http: AuthorizedHttpClient, private authService: AuthService) {
  }
  
  async initializeHubConnection() {
    
    var token!: string;
    try {
      console.log('Initializing SignalR connection');

      await this.authService.getToken.subscribe(t=> {token = t ?? ""});

      if (token != null && token != "") {

        if (!this.hubConnection || this.hubConnection.state == SignalR.HubConnectionState.Disconnected) {
          this.hubConnection = new HubConnectionBuilder()
            .withUrl(this.hubUrl, {
              accessTokenFactory: () => token,
              transport: SignalR.HttpTransportType.WebSockets
            })
            .build();
        }
      }
    }

    catch (error) {
      console.error('Error initializing SignalR connection: ', error);
    }
  }

  startConnection(): Observable<void> {
    return new Observable<void>((observer) => {
      this.hubConnection.start().then(() => {
        console.log('SignalR connection started');
        observer.next();
        observer.complete();
      })});
  }

  stopConnection(): Observable<void> {
    return new Observable<void>((observer) => {
      this.hubConnection.invoke('Disconnected').then(() => {
        observer.next();
        observer.complete();
      })
    });
  }

  getOnlineCustomers(): Observable<ChatUser[]> {
    return new Observable<ChatUser[]>(observer => {
      this.hubConnection.on('OnlineCostumers', (customers: ChatUser[]) => {
        observer.next(customers);
      });
    });
  }

  getOnlineSellers(): Observable<ChatUser[]> {
    return new Observable<ChatUser[]>(observer => {
      this.hubConnection.on('OnlineSellers', (sellers: ChatUser[]) => {
        observer.next(sellers);
      });
    });
  }

  getOwnChatUser(): Observable<ChatUser> {
    return new Observable<ChatUser>(observer => {
      this.hubConnection.on("OwnChatUser", (chatUser: ChatUser) => {
          observer.next(chatUser);
      })
    });
  }

  sendMessage(message: Message, convParticipants: ChatUser[]): void {
    this.hubConnection.invoke('SendPrivateMessage', message, convParticipants)
      .catch(err => console.error('Error while sending message: ', err));
  }

  receiveMessage(): Observable<Message> {
    return new Observable<Message>(observer => {
      this.hubConnection.on('ReceivePrivateMessage', (message: Message) => {
        observer.next(message);
      });
    });
  }

  receiveUserConversations(): Observable<Conversation[]> {
    return new Observable<Conversation[]>(observer => {
      this.hubConnection.on('ReceiveUserConversations', (conversations: Conversation[]) => {
        console.log(conversations);
        observer.next(conversations);
      })
    });
  }

  createConversation(participants: ChatUser[]): void {
    this.hubConnection.invoke('CreateConversation', participants)
      .catch(err => console.error('Error while creating conversation: ', err));
  }

  deleteConversation(conversation: Conversation): void {
    this.hubConnection.invoke('DeleteConversation', conversation)
      .catch(err => console.error('Error while deleting conversation: ', err));
  }

  
}
