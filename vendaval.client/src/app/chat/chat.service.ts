import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import * as SignalR from '@microsoft/signalr';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  onlineSellers = new BehaviorSubject<string[]>([]);
  constructor(private hubConnection: SignalR.HubConnection) { }
}
