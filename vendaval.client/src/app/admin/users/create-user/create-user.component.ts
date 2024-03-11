import { Component } from '@angular/core';
import { LoginService } from '../../../login/login.service';
import { User } from '../../../shared/common/interfaces/user';
import { UserType } from '../../../shared/common/enums/user-type';
import { lastValueFrom } from 'rxjs';
import { Router } from '@angular/router';
import { LoadingService } from '../../../shared/common/loading.service';

@Component({
  selector: 'app-create-user',
  templateUrl: './create-user.component.html',
  styleUrl: './create-user.component.css'
})
export class CreateUserComponent {

  user: User = {
    id: 0,
    name: '',
    email: '',
    password: '',
    userType: UserType.Costumer,
    birthDate: new Date(),
    phoneNumber: '',
    createdAt: null,
    updatedAt: null
  };

  hasError = false;
  error :string = '';

  constructor(private router: Router, private loadingService: LoadingService,private loginService: LoginService) {

  }

  async createUser() {
    this.loadingService.isLoading.next(true);
    try {

      this.user.userType = Number(this.user.userType);
      var res = await lastValueFrom(this.loginService.register(this.user));

      if (res.success) {
        this.hasError = false;
        this.error = '';
        this.router.navigate(['/admin/users']);
      }

      else {
        this.hasError = true;
        this.error = res.message;
      }
      this.loadingService.isLoading.next(false);
    }

    catch (e: any) {
      this.loadingService.isLoading.next(false);
      this.hasError = true;
      this.error = e.error.message;
    }
  }
}
