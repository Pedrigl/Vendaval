import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment'; 
import { Login, LoginResponse } from './login';
import { User } from './user';
@Injectable({
  providedIn: 'root'
})
export class LoginService {

  constructor(private httpClient: HttpClient) { }

  login(loginModel: Login) {
    this.httpClient.post<LoginResponse>(environment.apiUrl + 'User/login', loginModel);
  }

  register(user: User) {
    this.httpClient.post(environment.apiUrl + 'User/register', user);
  }

  put(user: User) {
    this.httpClient.put(environment.apiUrl + 'User/put', user);
  }

  patch(user: User) {
    this.httpClient.patch(environment.apiUrl + 'User/patch', user);
  }

  delete(id: Number) {
    this.httpClient.delete(environment.apiUrl + `User/delete?id=${id}`);
  }
}
