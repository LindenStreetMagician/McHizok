import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Subject, takeUntil } from 'rxjs';
import { Coupon } from 'src/app/models/coupon.model';
import { AuthService } from 'src/app/services/auth.service';
import { CouponInventoryService } from 'src/app/services/coupon-inventory.service';
import { downloadCoupon } from 'src/app/utilities/coupon-download-util';

@Component({
  selector: 'app-coupon-inventory',
  templateUrl: './coupon-inventory.component.html',
  styleUrls: ['./coupon-inventory.component.css']
})
export class CouponInventoryComponent implements OnInit {

  private ngUnsubscribe = new Subject;
  userCoupons: Coupon[] = [];

  constructor(private couponInventoryService: CouponInventoryService, private authService: AuthService, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.couponInventoryService.getUserCoupons(this.authService.getLoggedInUserId())
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (coupons) => {
          this.userCoupons = coupons;
        }
      });
  }

  onClickDelete(coupon: Coupon) {
    if (this.isCouponExpired(coupon.expiresAt)) {
      this.deleteCoupon(coupon);
      return;
    }

    if (window.confirm(`This coupon is still valid. Are you sure?`)) {
      this.deleteCoupon(coupon);
    }
  }

  onClickDownload(coupon: Coupon) {
    downloadCoupon(coupon);
  }

  onClickShowCoupon(coupon: Coupon) {
    coupon.showCoupon = !coupon.showCoupon;
  }

  private isCouponExpired(expirationTime: Date): boolean {
    const today = new Date().getTime();
    const expiry = new Date(expirationTime).getTime();

    return expiry <= today;
  }

  private deleteCoupon(coupon: Coupon) {
    console.log(coupon);
    this.couponInventoryService.deleteCoupon(coupon)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: () => {
          let index = this.userCoupons.indexOf(coupon);
          this.userCoupons.splice(index, 1);

          this.toastr.success('Coupon deleted.');
        }
      });
  }

  ngOnDestroy(): void {
    this.ngUnsubscribe.next(null);
    this.ngUnsubscribe.complete();
  }
}
