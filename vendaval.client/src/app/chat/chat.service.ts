import { Injectable, OnInit } from '@angular/core';
import { BehaviorSubject, Observable, lastValueFrom, map, throwError } from 'rxjs';
import * as SignalR from '@microsoft/signalr';
import { environment } from '../../environments/environment';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../shared/common/auth.service';
import { AuthorizedHttpClient } from '../shared/common/authorized-httpclient';

@Injectable({
  providedIn: 'root'
})
export class ChatService { 
  private hubConnection!: HubConnection;
  private messagesSubject = new BehaviorSubject<string[]>([]);
  messages$ = this.messagesSubject.asObservable();
  private hubUrl = environment.apiUrl + "chathub";
  constructor(private http: AuthorizedHttpClient, private authService: AuthService) {
  }
  //FIX MESSAGES NOT BEING RECEIVED
  public async initializeHubConnection() {
    var token!: string;
    try {
      console.log('Initializing SignalR connection');

      await this.authService.getToken.subscribe(t=> {token = t ?? ""});

      if (token != null && token != "") {
        console.log('Token: ' + token);
        this.hubConnection = new HubConnectionBuilder()
          .withUrl(this.hubUrl, {
            accessTokenFactory: () => token,
            transport: SignalR.HttpTransportType.WebSockets 
          })
          .build();

        await this.startConnection();

        this.hubConnection.on('ReceiveMessage', (senderName: string, message: string) => {
          this.messagesSubject.next([...this.messagesSubject.value, `${senderName}: ${message}`]);
        });
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

  getOnlineCustomers(): Observable<string[]> {
    return this.http.get<string[]>(`${environment.apiUrl}/Chat/onlineCustomers`);
  }

  getOnlineSellers(): Observable<string[]> {
    return this.http.get<string[]>(`${environment.apiUrl}/Chat/onlineSellers`);
  }

  sendMessage(message: string): void {
    this.hubConnection.invoke('SendMessage', message)
      .catch(err => console.error('Error while sending message: ', err));
  }



  getMessages(): string[] {
    return this.messagesSubject.value;
  }
}
