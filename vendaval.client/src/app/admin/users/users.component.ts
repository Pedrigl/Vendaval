import { Component } from '@angular/core';
import { User } from '../../login/user';
import { LoginService } from '../../login/login.service';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrl: './users.component.css'
})
export class UsersComponent {
    users: User[];
    constructor(private loginService: LoginService) {
        
     }
}
