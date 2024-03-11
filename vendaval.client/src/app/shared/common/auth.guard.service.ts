import { Injectable } from '@angular/core';
import { AuthService } from './auth.service';
import { ActivatedRoute, ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs/internal/Observable';
import { lastValueFrom, map } from 'rxjs';
import { UserType } from './enums/user-type';

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
          return this.router.createUrlTree(['/login'], { queryParams: { reason: "notloggedin" } });
        }

        const roles = route.data['roles'] as Array<UserType>;
        
        if (roles && !roles.includes(user.userType)) {
          return this.router.createUrlTree(['/notallowed']);
        }

        return true;
      })
    );
  }
}
