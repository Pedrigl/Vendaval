import { Component, OnInit } from '@angular/core';
import { User } from '../shared/common/interfaces/user';
import { UserType } from '../shared/common/enums/user-type';
import { AuthService } from '../shared/common/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrl: './menu.component.css'
})
export class MenuComponent implements OnInit{

  constructor(private router: Router,private authService: AuthService) { }

  navBarCollapsed: boolean = true;
  isLoggedIn: boolean = false;
  user: User = {
    id: 0,
    userType: UserType.Costumer,
    email: '',
    password: '',
    name: '',
    birthDate: new Date(),
    phoneNumber: '',
    address: [],
    createdAt: null,
    updatedAt: null
  }

  ngOnInit() {
    this.authService.isLoggedIn.subscribe(loggedIn => this.isLoggedIn = loggedIn);
  }

  public logout() {
    this.authService.logOut();
    this.router.navigate(['/login']);
  };

}
