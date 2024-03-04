import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable, lastValueFrom } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class RolesGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) { }

  async canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Promise<boolean | UrlTree> {

    const isLoggedIn = this.authService.isLoggedIn;

    if (!isLoggedIn) {
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
}
