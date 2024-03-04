import { Injectable, OnInit } from '@angular/core';
import {BehaviorSubject } from 'rxjs';
import { User } from '../../login/user';
import { Login } from '../../login/login';

@Injectable({
  providedIn: 'root'
})
export class AuthService{
  private loggedIn = new BehaviorSubject<boolean>(false);
  private user!: BehaviorSubject<User | null>;
  private login!: BehaviorSubject<Login | null>;
  private tokenExpiration!: BehaviorSubject<Date | null>;
  private token!: BehaviorSubject<string | null>;

  constructor() {
    var storedUser = sessionStorage.getItem('user') ?? localStorage.getItem('user');
    var storedLogin = sessionStorage.getItem('login') ?? localStorage.getItem('login');
    var storedTokenExpiration = sessionStorage.getItem('tokenExpiration') ?? localStorage.getItem('tokenExpiration');
    var storedToken = sessionStorage.getItem('token') ?? localStorage.getItem('token');
  
    this.user = new BehaviorSubject<User | null>(storedUser != null ? JSON.parse(storedUser) : null);
    this.login = new BehaviorSubject<Login | null>(storedLogin != null ? JSON.parse(storedLogin) : null);
    this.tokenExpiration = new BehaviorSubject<Date | null>(storedTokenExpiration != null ? new Date(storedTokenExpiration) : null);
    this.token = new BehaviorSubject<string | null>(storedToken != null ? storedToken : null);
  
    if(this.user.value != null && this.login.value != null && this.tokenExpiration.value != null && this.token.value != null) {
      this.loggedIn.next(true);
    }
  }

  get isLoggedIn() {
    return this.loggedIn.asObservable();
  }
  get getUser() {
    return this.user.asObservable();
  }

  get getLogin() {
    return this.login.asObservable();
  }

  get getTokenExpiration() {
    return this.tokenExpiration.asObservable();
  }

  get getToken() {
    return this.token.asObservable();
  }

  public setUser(user: User, keepLoggedIn: boolean) {
    this.user.next(user);
    sessionStorage.setItem('user', JSON.stringify(user))

    if(keepLoggedIn) {
      localStorage.setItem('user', JSON.stringify(user));
    }
  }

  public setTokenExpiration(expiration: Date, keepLoggedIn: boolean) {
    this.tokenExpiration.next(expiration);
    sessionStorage.setItem('tokenExpiration', expiration.toString());

    if(keepLoggedIn) {
      localStorage.setItem('tokenExpiration', expiration.toString());
    }
  }

  public setToken(token: string, keepLoggedIn: boolean) {
    this.token.next(token);
    sessionStorage.setItem('token', token);

    if (keepLoggedIn) {
      localStorage.setItem('token', token);
    }
  }

  public setLogin(login: Login, keepLoggedIn: boolean) {
    this.login.next(login);
    sessionStorage.setItem('login', JSON.stringify(login));

    if(keepLoggedIn) {
      localStorage.setItem('login', JSON.stringify(login));
    }
  }

  public logOut() {
    sessionStorage.clear();
    localStorage.clear();
    this.loggedIn.next(false);
  }

  public logIn() {
    this.loggedIn.next(true);
  }
}
