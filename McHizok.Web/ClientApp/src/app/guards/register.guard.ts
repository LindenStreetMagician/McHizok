import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable, map } from 'rxjs';
import { UserService } from '../services/user.service';

@Injectable({
  providedIn: 'root'
})
export class RegisterGuard implements CanActivate {
  constructor(private userService: UserService, private router: Router, private toastr: ToastrService) { }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    let registrationToken: string = route.params['regToken'];
    return this.userService.validateRegistrationToken(registrationToken).pipe(
      map(isRegistrationTokenValid => {
        if (isRegistrationTokenValid) {
          return true;
        }

        this.toastr.error('The Registration link is invalid.');
        this.router.navigate(['/login']);
        return false;
      })
    );

  }
}
