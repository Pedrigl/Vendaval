import { Injectable } from '@angular/core';
import { AuthService } from './auth.service';
import { ActivatedRoute, ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs/internal/Observable';
import { lastValueFrom, map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthGuardService implements CanActivate{

  constructor(private authService: AuthService, private router: Router) { }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> {
    return this.authService.getUser.pipe(
      map(user => {
        if (user == null) {
          return this.router.createUrlTree(['/login'], { queryParams: { reason: "you are not logged in" } });
        }

        const roles = route.data['roles'] as Array<string>;
        if (roles && !roles.includes(user.userType.toString())) {
          return this.router.createUrlTree(['/login'], { queryParams: { reason: `you don't have the permissions to access ${state.url}` } });
        }

        return true;
      })
    );
  }
}
