import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { User } from '../../shared/common/interfaces/user';
import { LoginService } from '../../login/login.service';
import { ApiResponse } from '../../shared/common/interfaces/apiResponse';
import { UserType } from '../../shared/common/enums/user-type';
import { lastValueFrom } from 'rxjs/internal/lastValueFrom';
import { LoadingService } from '../../shared/common/loading.service';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrl: './users.component.css'
})
export class UsersComponent {
    users!: ApiResponse<User[]>;
    filteredUsers!: ApiResponse<User[]>;
    userType = UserType;
    hasError = false;
    error = '';
    searchTerm: string = '';

  constructor(private router: Router, private loadingService: LoadingService, private loginService: LoginService) {
      loadingService.isLoading.next(true);
      loginService.getUsers().subscribe(response => {
        this.users = response;
        this.filteredUsers = { ...response, data: [...response.data] };
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

  filterList() {
    if (this.searchTerm) {
      this.filteredUsers.data = this.users.data.filter(item => item.name.toLowerCase().includes(this.searchTerm.toLowerCase()));
    } else {
      this.filteredUsers.data = [...this.users.data];
    }
  }


}
