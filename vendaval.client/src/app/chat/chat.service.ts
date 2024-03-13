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
  
  public async initializeHubConnection() {
    var token!: string;
    try {
      console.log('Initializing SignalR connection');

      await this.authService.getToken.subscribe(t=> {token = t ?? ""});

      if (token != null && token != "") {
        
        this.hubConnection = new HubConnectionBuilder()
          .withUrl(this.hubUrl, {
            accessTokenFactory: () => token,
            transport: SignalR.HttpTransportType.WebSockets 
          })
          .build();

        await this.startConnection();

        this.hubConnection.on("OwnChatUser", (d: ChatUser) => {
          if(d != null)
            this.ownChatUserSubject.next(d);
        });

        this.hubConnection.on("ReceivePrivateMessage", (message: Message) => {
          console.log(`receiving a message from ${message.SenderId} to ${message.ReceiverId}`)
          const currentMessages = [...this.messagesSubject.value];
          
          currentMessages.push(message);
          this.messagesSubject.next(currentMessages);
        })
      }
      
    } catch (error) {
      console.error('Error initializing SignalR connection: ', error);
    }
  }

  private async startConnection(): Promise<void> {
    try {
      if (this.hubConnection) {
        await this.hubConnection.start().then(() =>
        console.log('SignalR connection started'));
      } else {
        console.error('SignalR connection is not initialized');
      }
      console.log('SignalR connection status: ' + this.hubConnection.state);
    } catch (error) {
      console.error('Error while starting SignalR connection: ', error);
    }
  }

  getOnlineCustomers(): void {
    this.hubConnection.on("OnlineCostumers", (d: ChatUser[]) => {
      this.onlineCustomersSubject.next(d);
    });
  }
  
  sendMessage(message: Message): void {
    console.log(`sending a message from ${message.SenderId} to ${message.ReceiverId}`)
    this.hubConnection.invoke('SendPrivateMessage', message)
      .catch(err => console.error('Error while sending message: ', err));
  }


  
}
