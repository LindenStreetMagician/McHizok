import { Component, OnDestroy } from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import { Buffer } from 'buffer';
import { ApplePieService } from '../../services/apple-pie.service';
import { ToastrService } from 'ngx-toastr';
import { Coupon } from 'src/app/models/coupon.model';

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

  constructor(private applePieService: ApplePieService, private toastr: ToastrService) { }

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

    this.applePieService.getApplePie(hyphenFreeBlockCode).pipe(takeUntil(this.ngUnsubscribe)).subscribe(
      {
        next: (applePieCoupon) => {
          this.couponSrc = "data:image/jpeg;base64," + applePieCoupon.base64Content;
          this.coupon = applePieCoupon;
          this.downloadCoupon(applePieCoupon.fileName, this.convertBase64ToBlob(applePieCoupon.base64Content));
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

  ngOnDestroy(): void {
    this.ngUnsubscribe.next(null);
    this.ngUnsubscribe.complete();
  }
}
