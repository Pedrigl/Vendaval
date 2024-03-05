import { Component } from '@angular/core';
import { User } from '../../login/user';
import { LoginService } from '../../login/login.service';
import { ApiResponse } from '../../shared/common/interfaces/apiResponse';
import { UserType } from '../../login/user-type';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrl: './users.component.css'
})
export class UsersComponent {
    users!: ApiResponse<User[]>;
    userType = UserType;

    constructor(private loginService: LoginService) {
      loginService.getUsers().subscribe(response => {
        this.users = response;
      });
     }

     editUser(user: User) {
       
     }

     deleteUser(id: number) {
         console.log(id);
     }
}
