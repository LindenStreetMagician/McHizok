import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, tap } from 'rxjs';
import { environment } from 'src/environments/environment';
import { LoginRequest } from '../models/login-request.model';
import { LoginResult } from '../models/login-result.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private _baseUrl: string = environment.apiUrl;
  private _authStatus = new BehaviorSubject<boolean>(false);
  public authenticated$ = this._authStatus.asObservable();
  public tokenKey: string = 'token';

  constructor(private http: HttpClient) { }

  init() {
    if (this.isAuthenticated()) {
      this.setAuthStatus(true);
    }
  }


  login(loginRequest: LoginRequest) {
    return this.http.post(this._baseUrl + 'api/authentication/login', loginRequest).pipe(
      tap(
        (loginResult: LoginResult) => {
          if (!!loginResult.token) {
            localStorage.setItem(this.tokenKey, loginResult.token);
            this.setAuthStatus(true);
          }
        }
      )
    );
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  isAuthenticated(): boolean {
    let token = this.getToken();

    if (token == null) {
      return false;
    }

    if (this.tokenExpired(token)) {
      return false;
    }

    return true;
  }

  getLoggedInUserId() {
    if (!this.isAuthenticated) {
      return false;
    }

    let token = this.getToken();

    return this.getTokenDetails(token!)["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];
  }

  isInRole(role: string) {
    if (!this.isAuthenticated) {
      return false;
    }

    let token = this.getToken();

    let isJwtInRole = this.getTokenDetails(token!)["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] == role;

    return isJwtInRole;
  }

  logout() {
    localStorage.removeItem(this.tokenKey);
    this.setAuthStatus(false);
  }

  private getTokenDetails(token: string) {
    if (token == null) {
      this.logout();
      return "";
    }

    return JSON.parse(atob(token!.split('.')[1]));
  }

  private tokenExpired(token: string) {
    const expiry = this.getTokenDetails(token).exp;
    return (Math.floor((new Date).getTime() / 1000)) >= expiry;
  }

  private setAuthStatus(isAuthenticated: boolean): void {
    this._authStatus.next(isAuthenticated);
  }
}
