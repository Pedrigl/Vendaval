import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthorizedHttpClient } from '../shared/common/authorized-httpclient';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  constructor(private httpClient: HttpClient, private authClient: AuthorizedHttpClient) { }
}
