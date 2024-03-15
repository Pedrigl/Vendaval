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
  private messagesSubject = new BehaviorSubject<Message[]>([]);
  messages$ = this.messagesSubject.asObservable();

  private onlineCustomersSubject = new BehaviorSubject<ChatUser[]>([]);
  onlineCustomers$ = this.onlineCustomersSubject.asObservable();

  private ownChatUserSubject = new BehaviorSubject<ChatUser | null>(null);
  ownChatUser$ = this.ownChatUserSubject.asObservable();
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

  getOwnChatUser(): Observable<ChatUser> {
    return new Observable<ChatUser>(observer => {
      this.hubConnection.on("OwnChatUser", (d: ChatUser) => {
          observer.next(d);
      })
    });
  }

  sendMessage(message: Message): void {
    this.hubConnection.invoke('SendPrivateMessage', message)
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
        observer.next(conversations);
      })
    });
  }

  
}
