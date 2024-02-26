import { Injectable } from '@angular/core';
import {BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private loggedIn = new BehaviorSubject<boolean>(false);
  constructor() { }
  get isLoggedIn() {
    return this.loggedIn.asObservable();
  }

  public login() {
    this.loggedIn.next(true);
  }

  public logout() {
    this.loggedIn.next(false);
  }
}
