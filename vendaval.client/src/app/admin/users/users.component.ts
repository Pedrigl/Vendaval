import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { User } from '../../login/user';
import { LoginService } from '../../login/login.service';
import { ApiResponse } from '../../shared/common/interfaces/apiResponse';
import { UserType } from '../../login/user-type';
import { lastValueFrom } from 'rxjs/internal/lastValueFrom';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrl: './users.component.css'
})
export class UsersComponent {
    users!: ApiResponse<User[]>;
    userType = UserType;
    hasError = false;
    error = '';
    constructor(private router: Router, private loginService: LoginService) {
      loginService.getUsers().subscribe(response => {
        this.users = response;
      });
     }

    editUser(id: number) {
        this.router.navigate(['/admin/users/edit'], { queryParams: { id: id } });
     }

    async deleteUser(id: number) {
      try {
        var req = await lastValueFrom(this.loginService.deleteUser(id));

        if (!req.success) {
          this.hasError = true;
          this.error = req.message;
        }

        this.hasError = false;
        this.users.data = this.users.data.filter(user => user.id !== id);
      }
      catch (error: any) {
        this.hasError = true;
        this.error = error;
      }
      
      }
}
