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
    return this.getToken() !== null;
  }

  init() {
    if (this.isAuthenticated()) {
      this.setAuthStatus(true);
    }
  }

  private setAuthStatus(isAuthenticated: boolean): void {
    this._authStatus.next(isAuthenticated);
  }

  logout() {
    localStorage.removeItem(this.tokenKey);
    this.setAuthStatus(false);
  }
}
