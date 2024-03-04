import { ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable, lastValueFrom } from 'rxjs';
import { User } from '../../login/user';
import { AuthService } from './auth.service';

export async function canActivateRoles(expectedRoles: string[], authService: AuthService, router: Router): Promise<(next: ActivatedRouteSnapshot, state: RouterStateSnapshot) => Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree> {
  return async (next: ActivatedRouteSnapshot, state: RouterStateSnapshot) => {
    const isLoggedIn = authService.isLoggedIn;
    if (!isLoggedIn) {
      return router.parseUrl('/login');
    }

    var user = authService.GetUser();
    var userValue = await lastValueFrom(user);

    if (userValue == null||) {
      return router.parseUrl('/login');
    }
    return true;
  };
}
