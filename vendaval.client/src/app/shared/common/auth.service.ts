import { Injectable, OnInit } from '@angular/core';
import {BehaviorSubject } from 'rxjs';
import { User } from '../../login/user';
import { Login } from '../../login/login';

@Injectable({
  providedIn: 'root'
})
export class AuthService implements OnInit{
  private loggedIn = new BehaviorSubject<boolean>(false);
  private user!: BehaviorSubject<User | null>;
  private login!: BehaviorSubject<Login | null>;
  private tokenExpiration!: BehaviorSubject<Date | null>;

  constructor() { }

  ngOnInit() {
    var storedUser = sessionStorage.getItem('user') ?? localStorage.getItem('user');
    var storedLogin = sessionStorage.getItem('login') ?? localStorage.getItem('login');
    var storedTokenExpiration = sessionStorage.getItem('tokenExpiration') ?? localStorage.getItem('tokenExpiration');

    this.user = storedUser != null ? JSON.parse(storedUser) : new BehaviorSubject<User | null>(null);
    this.login = storedLogin != null ? JSON.parse(storedLogin) : new BehaviorSubject<Login | null>(null);
    this.tokenExpiration = storedTokenExpiration != null ? new BehaviorSubject<Date | null>(new Date(storedTokenExpiration)) : new BehaviorSubject<Date | null>(null);

    if(this.user != null && this.login != null && this.tokenExpiration != null) {
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
