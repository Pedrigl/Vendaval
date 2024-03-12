import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Login, LoginResponse } from './login';
import { User } from '../shared/common/interfaces/user';
import { UserType } from '../shared/common/enums/user-type';
import { LoginService } from './login.service';
import { lastValueFrom } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { AuthService } from '../shared/common/auth.service';
import { LoadingService } from '../shared/common/loading.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit{

  constructor(private router: Router, private route: ActivatedRoute,private loadingService: LoadingService, private loginService: LoginService, private authService: AuthService) { }

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
    createdAt: null,
    updatedAt: null
  };

  async ngOnInit() {
    this.checkRedirect();
    await this.checkIfUserIsLoggedInAsync();
  }

  public async loginAsync() {
    this.loadingService.isLoading.next(true);
    this.hasLoginError = false;
    this.loginError = '';
    var loginRes: LoginResponse;

    try {
      const loginReq = await this.loginService.login(this.login);
      loginRes = await lastValueFrom(loginReq);
    }

    catch (error: any) {
      this.hasLoginError = true;
      this.loadingService.isLoading.next(false);
      if (error instanceof HttpErrorResponse) {
        var errorParse : LoginResponse = error.error;
        this.loginError = errorParse.message;
      }
      else {
        this.loginError = error.error;
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
    this.authService.setToken(loginRes.token, this.keepUserLoggedIn);
    this.authService.setTokenExpiration(currentDate, this.keepUserLoggedIn);
    this.loadingService.isLoading.next(false);
    this.authService.logIn();
    this.redirectBasedOnUserType(loginRes.user.userType);
  };

  private async redirectBasedOnUserType(userType: UserType) {
    switch (userType) {
      case UserType.Costumer:
        this.router.navigate(['/home']);
        break;

      case UserType.Seller:
        this.router.navigate(['/chat']);
          break;

      case UserType.Admin:
        this.router.navigate(['/admin']);
        break;
    }
  }

  public async registerAsync() {
    this.loadingService.isLoading.next(true);
    var loginRes: LoginResponse;

    try {
      const loginReq = await this.loginService.register(this.user);
      loginRes = await lastValueFrom(loginReq);
    }

    catch (error: any) {
      this.loadingService.isLoading.next(false);
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
    this.loadingService.isLoading.next(false);

  };

  public async checkIfUserIsLoggedInAsync() {
    const login = this.authService.getLogin;
    const user = this.authService.getUser;
    const token = this.authService.getToken;
    const tokenExpiration = this.authService.getTokenExpiration;

    if (login == null || user == null || token == null || tokenExpiration == null) {
      this.loadingService.isLoading.next(false);
      return;
    }
    
    const tokenExpirationValue = await lastValueFrom(tokenExpiration) ;

    if (tokenExpirationValue == null) {
      return;
    }
  
    if (tokenExpirationValue < new Date()) {
      this.authService.logOut();
      return;
    }
    this.authService.logIn();

    this.router.navigate(['/home']);
  };

  private checkRedirect() {
    this.route.queryParams.subscribe(params => {
      let redirectReason = params['reason'];
      if (redirectReason != null) {
        if (redirectReason == "notloggedin") {
          this.hasLoginError = true;
          this.loginError = "You need to be logged in to access this page";
        }
    }});
  }
  
}
