import { Component, OnDestroy } from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import { Buffer } from 'buffer';
import { ApplePieService } from '../../services/apple-pie.service';
import { ToastrService } from 'ngx-toastr';
import { Coupon } from 'src/app/models/coupon.model';
import { CouponInventoryService } from 'src/app/services/coupon-inventory.service';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-apple-pie',
  templateUrl: './apple-pie.component.html',
  styleUrls: ['./apple-pie.component.css']
})
export class ApplePieComponent implements OnDestroy {
  public blockCode: string = "";
  public coupon: Coupon | undefined = undefined;
  public couponSrc = "";
  private ngUnsubscribe = new Subject;
  private regex = /^[a-zA-Z0-9]{12}$/;

  constructor(private applePieService: ApplePieService,
    private authService: AuthService,
    private couponInventoryService: CouponInventoryService,
    private toastr: ToastrService) { }

  onClickSaveCoupon() {
    this.couponInventoryService.saveCouponForUser(this.authService.getLoggedInUserId(), this.coupon!)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: () => {
          this.toastr.success("Coupon has been saved!");
          this.resetCouponState();
        }
      });
  }

  onClickDownloadCoupon() {
    this.downloadCoupon(this.coupon!.fileName, this.convertBase64ToBlob(this.coupon!.base64Content));
  }

  onClickUsedCoupon() {
    if (window.confirm(`Redeemed the coupon? If you click OK it will be lost.`)) {
      this.resetCouponState();
    }
  }

  onClickGetApplePie() {
    if (this.blockCode == "") {
      this.toastr.error("The code field is empty.");
      return;
    }

    let hyphenFreeBlockCode = this.blockCode.replaceAll('-', '');

    let isCodeValid = this.regex.test(hyphenFreeBlockCode);

    if (!isCodeValid) {
      this.toastr.error("Invalid code format.");
      return;
    }

    this.applePieService.getApplePie(hyphenFreeBlockCode)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (applePieCoupon) => {
          this.couponSrc = "data:image/jpeg;base64," + applePieCoupon.base64Content;
          this.coupon = applePieCoupon;
        },
        complete: () => {
          this.blockCode = "";
        }
      });
  }

  private downloadCoupon(couponName: string, couponContent: Blob) {
    const a = document.createElement('a');
    const objectUrl = URL.createObjectURL(couponContent);
    a.href = objectUrl;
    a.download = couponName;
    a.click();
    URL.revokeObjectURL(objectUrl);
  }

  private convertBase64ToBlob(base64Coupon: string): Blob {
    const byteCharacters = Buffer.from(base64Coupon, 'base64').toString('binary');

    const byteNumbers = new Array(byteCharacters.length);

    for (let i = 0; i < byteCharacters.length; i++) {
      byteNumbers[i] = byteCharacters.charCodeAt(i);
    }

    return new Blob([new Uint8Array(byteNumbers)]);
  }

  private resetCouponState() {
    this.coupon = undefined;
    this.couponSrc = "";
  }

  ngOnDestroy(): void {
    this.ngUnsubscribe.next(null);
    this.ngUnsubscribe.complete();
  }
}
