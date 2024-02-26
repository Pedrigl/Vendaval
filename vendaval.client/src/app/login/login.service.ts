import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment'; 
import { Login, LoginResponse } from './login';
import { User } from './user';
import { AuthorizedHttpClient } from '../shared/common/authorized-httpclient';
@Injectable({
  providedIn: 'root'
})
export class LoginService {

  constructor(private httpClient: HttpClient, private authClient: AuthorizedHttpClient) { }

  login(loginModel: Login) {
    return this.httpClient.post<LoginResponse>(environment.apiUrl + 'User/login', loginModel);
  }

  register(user: User) {
    return this.httpClient.post(environment.apiUrl + 'User/register', user);
  }

  putUser(user: User) {
    return this.authClient.put(environment.apiUrl + 'User/put', user);
  }

  patchUser(user: User) {
    return this.authClient.patch(environment.apiUrl + 'User/patch', user);
  }

  deleteUser(id: Number) {
    return this.authClient.delete(environment.apiUrl + `User/delete?id=${id}`);
  }
}
