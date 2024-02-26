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

  constructor() { }

  ngOnInit() {
    var storedUser = sessionStorage.getItem('user') ?? localStorage.getItem('user');

    if(storedUser == null) {
      this.user = new BehaviorSubject<User|null>(null);
      return;
    }

    this.user = JSON.parse(storedUser);
  }

  get isLoggedIn() {
    return this.loggedIn.asObservable();
  }
  public GetUser() {
    return this.user.asObservable();
  }

  public setUser(user: User, keepLoggedIn: boolean) {
    this.user.next(user);
    sessionStorage.setItem('user', JSON.stringify(user))

    if(keepLoggedIn) {
      localStorage.setItem('user', JSON.stringify(user));
    }
  }

  public logIn() {
    this.loggedIn.next(true);
  }

  public logOut() {
    this.loggedIn.next(false);
  }
}
