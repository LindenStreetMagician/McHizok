import { Component, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';
import { LoginRequest } from '../../models/login-request.model';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnDestroy {
  public loginRequest = <LoginRequest>{};
  private ngUnsubscribe = new Subject;

  constructor(public authService: AuthService,
    private router: Router) { }

  login() {
    this.authService.login(this.loginRequest).pipe(takeUntil(this.ngUnsubscribe)).subscribe({
      next: () => {
        this.router.navigate(['/']);
      }
    })
  }

  ngOnDestroy(): void {
    this.ngUnsubscribe.next(null);
    this.ngUnsubscribe.complete();
  }
}
