import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable, lastValueFrom } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class RolesGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) { }
  isLoggedIn: boolean = false;

  async canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Promise<boolean | UrlTree> {

    try {
      this.authService.isLoggedIn.subscribe(loggedIn => this.isLoggedIn = loggedIn);
      
      if (!this.isLoggedIn) {
        const navigationExtras = { queryParams: { reason: "notLoggedIn" } };
        return this.router.createUrlTree(['/login'], navigationExtras);
      }

      var user = this.authService.GetUser();

      var userValue = await lastValueFrom(user);

      const expectedRoles = next.data['roles'] as string[];

      if (userValue == null || !expectedRoles.includes(userValue.userType.toString())) {
        const navigationExtras = { queryParams: { reason: "noPermission" } };
        return this.router.createUrlTree(['/login'], navigationExtras);
      }

      return true;
    }
    catch (error) {
      console.log(error);
      return false;
    }
  }
}
