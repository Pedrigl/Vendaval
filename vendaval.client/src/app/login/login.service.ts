import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment'; 
import { Login, LoginResponse } from './login';
import { User } from './user';
import { AuthorizedHttpClient } from '../shared/common/authorized-httpclient';
import { ApiResponse } from '../shared/common/interfaces/apiResponse';
@Injectable({
  providedIn: 'root'
})
export class LoginService {

  constructor(private httpClient: HttpClient, private authClient: AuthorizedHttpClient) { }

  login(loginModel: Login) {
    return this.httpClient.post<LoginResponse>(environment.apiUrl + 'User/login', loginModel);
  }

  register(user: User) {
    return this.httpClient.post<LoginResponse>(environment.apiUrl + 'User/register', user);
  }

  putUser(user: User) {
    return this.authClient.put<LoginResponse>(environment.apiUrl + 'User/put', user);
  }

  patchUser(user: User) {
    return this.authClient.patch<LoginResponse>(environment.apiUrl + 'User/patch', user);
  }

    getUsers() {
        return this.authClient.get<ApiResponse<User[]>>(environment.apiUrl + 'User/get');
    }

  deleteUser(id: Number) {
    return this.authClient.delete<LoginResponse>(environment.apiUrl + `User/delete?id=${id}`);
  }
}
