import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Coupon } from '.././models/coupon.model';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ApplePieService {
  private _baseUrl: string = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getApplePie(blockCode: string): Observable<Coupon> {
    return this.http.get<Coupon>(this._baseUrl + 'api/applepies',
      {
        params: new HttpParams().set('blockCode', blockCode)
      });
  }
}
