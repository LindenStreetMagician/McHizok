import { Injectable } from '@angular/core';
import {
  Resolve,
  RouterStateSnapshot,
  ActivatedRouteSnapshot,
  Router
} from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { catchError, Observable, of } from 'rxjs';
import { User } from '../models/user.model';
import { UserService } from '../services/user.service';

@Injectable({
  providedIn: 'root'
})
export class UsersResolver implements Resolve<User[] | null> {

  constructor(private userService: UserService,
    private toastr: ToastrService,
    private router: Router) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<User[] | null> {
    return this.userService.getUsers().pipe(catchError(() => {
      this.toastr.error('Something went wrong :(');
      this.router.navigate(['/']);
      return of(null);
    }));
  }
}
