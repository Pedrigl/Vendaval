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

  constructor(private router: Router, private loginService: LoginService) {
      loginService.getUsers().subscribe(response => {
        this.users = response;
        this.filteredUsers = { ...response, data: [...response.data] };
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
        this.filteredUsers.data = this.filteredUsers.data.filter(user => user.id !== id);
      }
      catch (error: any) {
        
        this.hasError = true;
        this.error = error.error.message;
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
