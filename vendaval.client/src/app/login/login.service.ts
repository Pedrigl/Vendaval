import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment'; 
import { Login, LoginResponse } from './login';
@Injectable({
  providedIn: 'root'
})
export class LoginService {

  constructor(private httpClient: HttpClient) { }

  login(loginModel: Login) {
    this.httpClient.post<LoginResponse>(environment.apiUrl + 'login', loginModel);
  }
}
