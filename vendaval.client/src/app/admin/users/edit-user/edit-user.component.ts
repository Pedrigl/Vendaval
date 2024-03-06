import { Component, Input, OnInit } from '@angular/core';
import { LoginService } from '../../../login/login.service';
import { User } from '../../../login/user';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiResponse } from '../../../shared/common/interfaces/apiResponse';
import { Observable } from 'rxjs/internal/Observable';
import { lastValueFrom } from 'rxjs/internal/lastValueFrom';

@Component({
  selector: 'app-edit-user',
  templateUrl: './edit-user.component.html',
  styleUrl: './edit-user.component.css'
})
export class EditUserComponent{
    user!: User;
    hasError = false;
    error = '';

    constructor(private route: ActivatedRoute, private router: Router,private loginService: LoginService){
        
        this.loginService.getUser(this.route.snapshot.queryParams['id']).subscribe(response => {
          this.user = response.data;
        })
    }
    async saveUser() {
      try {
        this.user.userType = Number(this.user.userType);
        var req = await lastValueFrom(this.loginService.putUser(this.user));
        this.user = req.user;
        this.hasError = false;
        this.error = req.message;
        this.router.navigate(['/admin/users']);
        }

        catch (error: any) {
        this.hasError = true;

        this.error = error;
        }
        
    }

}
