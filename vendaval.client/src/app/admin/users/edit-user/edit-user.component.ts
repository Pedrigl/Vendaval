import { Component, Input, OnInit } from '@angular/core';
import { LoginService } from '../../../login/login.service';
import { User } from '../../../shared/common/interfaces/user';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiResponse } from '../../../shared/common/interfaces/apiResponse';
import { Observable } from 'rxjs/internal/Observable';
import { lastValueFrom } from 'rxjs/internal/lastValueFrom';
import { LoadingService } from '../../../shared/common/loading.service';

@Component({
  selector: 'app-edit-user',
  templateUrl: './edit-user.component.html',
  styleUrl: './edit-user.component.css'
})
export class EditUserComponent{
    user!: User;
    hasError = false;
    error = '';

    constructor(private route: ActivatedRoute, private router: Router, private loadingService: LoadingService,private loginService: LoginService){
        loadingService.isLoading.next(true);
        this.loginService.getUser(this.route.snapshot.queryParams['id']).subscribe(response => {
          this.user = response.data;
          loadingService.isLoading.next(false);
        })
    }
  async saveUser() {
      this.loadingService.isLoading.next(true);
      this.hasError = false;
      try {
          this.user.userType = Number(this.user.userType);
          var req = await lastValueFrom(this.loginService.putUser(this.user));
          this.user = req.user;

        this.loadingService.isLoading.next(false);
          if (!req.success) {
            this.hasError = true;
            this.error = req.message;
          }

          else {
            this.router.navigate(['/admin/users']);
          }
        }

      catch (error: any) {
        this.loadingService.isLoading.next(false);
        this.hasError = true;

        this.error = error;
        }
        
    }

}
