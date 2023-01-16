import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { LoginRequest } from '../../models/login-request.model';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  public loginRequest = <LoginRequest>{};

  constructor(public authService: AuthService, private toastr: ToastrService, private router: Router) { }

  login() {
    this.authService.login(this.loginRequest).subscribe({
      next: () => {
        this.router.navigate(['/']);
      },
      error: (err: HttpErrorResponse) => {
        if (err.status == 401) {
          this.toastr.error("Invalid username or password.");
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
