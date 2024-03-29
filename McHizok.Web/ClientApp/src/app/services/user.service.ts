import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { RegisterRequest } from '../models/register-request.model';
import { User } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private _baseUrl: string = environment.apiUrl;

  constructor(private http: HttpClient) { }

  generateRegistrationLink(accountFor: string): Observable<string> {
    return this.http.get(this._baseUrl + 'api/users/generate',
      {
        responseType: "text",
        params: new HttpParams().set('account_for', accountFor)
      });
  }

  validateRegistrationToken(regToken: string): Observable<boolean> {
    return this.http.get<boolean>(this._baseUrl + 'api/users/validate',
      {
        params: new HttpParams().set('token', regToken)
      });
  }

  registerUser(registerRequest: RegisterRequest) {
    return this.http.post(this._baseUrl + 'api/users/register', registerRequest);
  }

  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(this._baseUrl + "api/users");
  }

  deleteUser(userId: string) {
    return this.http.delete(this._baseUrl + `api/users`, {
      params: new HttpParams().set('userId', userId)
    });
  }
}
