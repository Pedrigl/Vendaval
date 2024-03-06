import { Component } from '@angular/core';
import { LoginService } from '../../../login/login.service';
import { User } from '../../../login/user';
import { UserType } from '../../../login/user-type';
import { lastValueFrom } from 'rxjs';
import { Router } from '@angular/router';

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
    address: []
  };

  hasError = false;
  error :string = '';

  constructor(private router: Router,private loginService: LoginService) {

  }

  async createUser() {
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
    }

    catch (e: any) {
      this.hasError = true;
      console.log(e);
      this.error = e.error.message;
    }
  }
}
