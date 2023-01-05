import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { LoginRequest } from '../models/login-request.model';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  public loginRequest = <LoginRequest>{};

  constructor(public authService: AuthService, private toastr: ToastrService) { }

  login() {
    this.authService.login(this.loginRequest).subscribe({
      next: (token) => { },
      error: (err: HttpErrorResponse) => {
        if (err.status == 401) {
          this.toastr.error("Hibás felhasználó név vagy jelszó.");
        }
        else {
          throw err;
        }
      }
    })
  }

  logout() {
    this.authService.logout();
  }
}
