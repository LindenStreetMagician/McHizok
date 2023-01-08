import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private _baseUrl: string = environment.apiUrl;

  constructor(private http: HttpClient) { }

  generateRegistrationLink(to: string): Observable<string> {
    return this.http.get(this._baseUrl + 'api/users/generate',
      {
        responseType: "text",
        params: new HttpParams().set('to', to)
      }
    );
  }

  validateRegistrationToken(regToken: string): Observable<boolean> {
    return this.http.get<boolean>(this._baseUrl + 'api/users/validate',
      {
        params: new HttpParams().set('token', regToken)
      });
  }
}
