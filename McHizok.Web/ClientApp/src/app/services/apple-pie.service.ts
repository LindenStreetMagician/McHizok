import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Coupon } from '.././models/coupon.model';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ApplePieService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getApplePie(blockCode: string): Observable<Coupon> {
    return this.http.get<Coupon>(this.baseUrl + 'api/applepies/dummy',
      {
        params: new HttpParams().set('blockCode', blockCode)
      });
  }
}
