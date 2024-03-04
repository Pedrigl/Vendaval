import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Login, LoginResponse } from './login';
import { User } from './user';
import { UserType } from './user-type';
import { LoginService } from './login.service';
import { lastValueFrom } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { AuthService } from '../shared/common/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit{

  constructor(private router: Router, private route: ActivatedRoute, private loginService: LoginService, private authService: AuthService) { }

  keepUserLoggedIn: boolean = false;

  hasLoginError: boolean = false;
  loginError: string = '';

  hasRegisterError: boolean = false;
  registerError: string = '';

  login: Login = {
    email: '',
    password: ''
  };

  user: User = {
    id: 0,
    userType: UserType.Costumer,
    email: '',
    password: '',
    name: '',
    birthDate: new Date(),
    phoneNumber: '',
    address: []
  };

  async ngOnInit() {
    this.checkRedirect();
    await this.checkIfUserIsLoggedInAsync();
  }

  public async loginAsync() {

    var loginRes: LoginResponse;

    try {
      const loginReq = await this.loginService.login(this.login);
      loginRes = await lastValueFrom(loginReq);
    }

    catch (error: any) {
      this.hasLoginError = true;

      if (error instanceof HttpErrorResponse) {
        var errorParse : LoginResponse = error.error;
        this.loginError = errorParse.message;
      }
      else {
        this.loginError = error;
      }

      return
    }

    if(!loginRes.success) {
      this.hasLoginError = true;
      this.loginError = loginRes.message;
      return;
    }
    let currentDate = new Date();
    currentDate.setDate(currentDate.getDate() + 1);


    this.authService.setUser(loginRes.user, this.keepUserLoggedIn);
    this.authService.setLogin(this.login, this.keepUserLoggedIn);
    this.authService.setTokenExpiration(currentDate, this.keepUserLoggedIn);

    this.authService.logIn();
    this.router.navigate(['/home']);
  };

  public async registerAsync() {
    var loginRes: LoginResponse;

    try {
      const loginReq = await this.loginService.register(this.user);
      loginRes = await lastValueFrom(loginReq);
    }

    catch (error: any) {
      this.hasRegisterError = true;

      if (error instanceof HttpErrorResponse) {
        var errorParse: LoginResponse = error.error;
        this.registerError = errorParse.message;
      }
      else {
        this.registerError = error;
      }

      return
    }

    if(!loginRes.success) {
      this.hasRegisterError = true;
      this.registerError = loginRes.message;
      return;
    }

  };

  public async checkIfUserIsLoggedInAsync() {
    const login = localStorage.getItem('login');
    const user = localStorage.getItem('user');
    const token = localStorage.getItem('token');

    if (login != null && user != null && token != null) {
      this.login = JSON.parse(login);
      this.user = JSON.parse(user);
      
      this.keepUserLoggedIn = true;
      this.authService.logIn();
      this.router.navigate(['/home']);
      return;
    }

    const sessionToken = sessionStorage.getItem('token');
    const sessionUser = sessionStorage.getItem('user');

    if (sessionToken != null && sessionUser != null) {
      this.authService.logIn();
      this.router.navigate(['/home']);
      return;
    }
    
  };

  private checkRedirect() {
    this.route.queryParams.subscribe(params => {
      let redirectReason = params['reason'];
      if (redirectReason != null) {
        if (redirectReason == "notLoggedIn") {
          this.hasLoginError = true;
          this.loginError = "You need to be logged in to access this page";
        }
        else if (redirectReason == "noPermission") {
          this.hasLoginError = true;
          this.loginError = "You don't have permission to access this page";
        }
      }
    });
  }
  
}
