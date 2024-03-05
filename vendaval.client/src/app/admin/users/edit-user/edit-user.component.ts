import { Component } from '@angular/core';
import { LoginService } from '../../../login/login.service';
import { User } from '../../../login/user';

@Component({
  selector: 'app-edit-user',
  templateUrl: './edit-user.component.html',
  styleUrl: './edit-user.component.css'
})
export class EditUserComponent {
    user!: User;

    constructor(private loginService: LoginService){

    }

    saveUser() {
        console.log(this.user);
    }

}
