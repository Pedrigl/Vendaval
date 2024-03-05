import { Component } from '@angular/core';
import { Router } from '@angular/router';
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

    constructor(private router: Router, private loginService: LoginService) {
      loginService.getUsers().subscribe(response => {
        this.users = response;
      });
     }

    editUser(id: number) {
        this.router.navigate(['/admin/users/edit'], { queryParams: { id: id } });
     }

     deleteUser(id: number) {
         console.log(id);
     }
}
