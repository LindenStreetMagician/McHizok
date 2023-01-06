import { Component, OnDestroy } from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import { Buffer } from 'buffer';
import { ApplePieService } from '../../services/apple-pie.service';

@Component({
  selector: 'app-apple-pie',
  templateUrl: './apple-pie.component.html',
  styleUrls: ['./apple-pie.component.css']
})
export class ApplePieComponent implements OnDestroy {
  public blockCode: string = "";
  private ngUnsubscribe = new Subject;

  constructor(private applePieService: ApplePieService) { }

  onClickGetApplePie() {
    this.applePieService.getApplePie(this.blockCode).pipe(takeUntil(this.ngUnsubscribe)).subscribe(
      {
        next: (applePieCoupon) => {
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
