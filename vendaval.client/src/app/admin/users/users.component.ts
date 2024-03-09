import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { User } from '../../login/user';
import { LoginService } from '../../login/login.service';
import { ApiResponse } from '../../shared/common/interfaces/apiResponse';
import { UserType } from '../../login/user-type';
import { lastValueFrom } from 'rxjs/internal/lastValueFrom';
import { LoadingService } from '../../shared/common/loading.service';

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
  constructor(private router: Router, private loadingService: LoadingService, private loginService: LoginService) {
      loadingService.isLoading.next(true);
      loginService.getUsers().subscribe(response => {
        this.users = response;
        loadingService.isLoading.next(false);
      });
     }

    editUser(id: number) {
        this.router.navigate(['/admin/users/edit'], { queryParams: { id: id } });
     }

  async deleteUser(id: number) {
      this.loadingService.isLoading.next(true);
      try {
        var req = await lastValueFrom(this.loginService.deleteUser(id));

        if (!req.success) {
          this.hasError = true;
          this.error = req.message;
        }
        this.loadingService.isLoading.next(false);
        this.hasError = false;
        this.users.data = this.users.data.filter(user => user.id !== id);
      }
      catch (error: any) {
        this.loadingService.isLoading.next(false);
        this.hasError = true;
        this.error = error;
      }
      
      }
}
