import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Login, LoginResponse } from './login';
import { User } from './user';
import { UserType } from './user-type';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  login: Login = {
    email: '',
    password: ''
  };

  user: User = {
    id: 0,
    userType: UserType.Costumer,
    email: '',
    password: '',
    name: '',
    birthDate: new Date(),
    phoneNumber: '',
    address: []
  };
}
