import { Component, OnInit } from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import { Coupon } from 'src/app/models/coupon.model';
import { AuthService } from 'src/app/services/auth.service';
import { CouponInventoryService } from 'src/app/services/coupon-inventory.service';

@Component({
  selector: 'app-coupon-inventory',
  templateUrl: './coupon-inventory.component.html',
  styleUrls: ['./coupon-inventory.component.css']
})
export class CouponInventoryComponent implements OnInit {

  private ngUnsubscribe = new Subject;
  userCoupons: Coupon[] = [];

  constructor(private couponInventoryService: CouponInventoryService, private authService: AuthService) { }

  ngOnInit(): void {
    this.couponInventoryService.getUserCoupons(this.authService.getLoggedInUserId())
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (coupons) => {
          this.userCoupons = coupons;
          console.log(this.userCoupons);
        }
      });
  }

  onClickDelete(couponId: string) {

  }

  onClickDownload(coupon: Coupon) {

  }

  onRowClick(coupon: Coupon) {
    console.log(coupon);
  }

  ngOnDestroy(): void {
    this.ngUnsubscribe.next(null);
    this.ngUnsubscribe.complete();
  }
}
