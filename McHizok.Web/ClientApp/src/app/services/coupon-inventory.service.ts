import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Coupon } from '../models/coupon.model';

@Injectable({
  providedIn: 'root'
})
export class CouponInventoryService {
  private _baseUrl: string = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getUserCoupons(userId: string): Observable<Coupon[]> {
    return this.http.get<Coupon[]>(this._baseUrl + 'api/applepies/coupons',
      {
        params: new HttpParams().set('userId', userId)
      });
  }

  saveCouponForUser(userId: string, coupon: Coupon) {
    coupon.userId = userId;

    return this.http.post<Coupon>(this._baseUrl + 'api/applepies/coupons', coupon);
  }

  deleteCoupon(couponId: string) {
    return this.http.delete(this._baseUrl + "api/applepies/coupons",
      {
        params: new HttpParams().set('couponId', couponId)
      });
  }
}
