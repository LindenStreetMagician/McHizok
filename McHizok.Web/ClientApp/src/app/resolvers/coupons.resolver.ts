import { Injectable } from '@angular/core';
import {
  Resolve,
  RouterStateSnapshot,
  ActivatedRouteSnapshot,
  Router
} from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { catchError, Observable, of } from 'rxjs';
import { Coupon } from '../models/coupon.model';
import { AuthService } from '../services/auth.service';
import { CouponInventoryService } from '../services/coupon-inventory.service';

@Injectable({
  providedIn: 'root'
})
export class CouponsResolver implements Resolve<Coupon[] | null> {

  constructor(private couponInventoryService: CouponInventoryService,
    private authService: AuthService,
    private toastr: ToastrService,
    private router: Router) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<Coupon[] | null> {
    return this.couponInventoryService.getUserCoupons(this.authService.getLoggedInUserId()).pipe(catchError(() => {
      this.toastr.error('Something went wrong :(');
      this.router.navigate(['/']);
      return of(null);
    }));
  }
}
