import { Component, Input, OnInit } from '@angular/core';
import { LoginService } from '../../../login/login.service';
import { User } from '../../../login/user';
import { ActivatedRoute } from '@angular/router';
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

    constructor(private route: ActivatedRoute,private loginService: LoginService){
        
        this.loginService.getUser(this.route.snapshot.queryParams['id']).subscribe(response => {
            this.user = response.data;
        })
    }
    async saveUser() {
        try {
            var req = await lastValueFrom(this.loginService.putUser(this.user));
            this.user = req.user;
            this.hasError = false;
        }

        catch (error:any) {
            this.hasError = true;
            console.log(error);
            this.error = error;
        }
        
    }

}
