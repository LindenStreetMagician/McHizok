import { Component, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subject, takeUntil } from 'rxjs';
import { RegisterRequest } from 'src/app/models/register-request.model';
import { AuthService } from 'src/app/services/auth.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnDestroy {
  public registerRequest: RegisterRequest = <RegisterRequest>{};
  public registrationToken: string = '';
  public repeatPassword: string = '';
  private ngUnsubscribe = new Subject;

  constructor(private toastr: ToastrService,
    private route: ActivatedRoute,
    private router: Router,
    private userService: UserService,
    private authService: AuthService) {
    this.registerRequest.registrationToken = this.route.snapshot.params['regToken'];
  }

  register() {
    if (this.registerRequest.userName.length < 6 || this.registerRequest.userName.length > 15) {
      this.toastr.error('The username must be at least 6 characters, but less than 15.');
      return;
    }

    if (this.registerRequest.password.length < 10) {
      this.toastr.error('The password length must be least 10 characters.');
      return;
    }

    if (!/[0-9]/.test(this.registerRequest.password)) {
      this.toastr.error('The password must contain at least 1 number.');
      return;
    }

    if (this.registerRequest.password !== this.repeatPassword) {
      this.toastr.error('The given passwords do not match.');
      return;
    }

    this.userService.registerUser(this.registerRequest).pipe(takeUntil(this.ngUnsubscribe)).subscribe({
      next: () => {
        this.authService.logout();
        this.router.navigate(['/login']);
      },
    })
  }

  ngOnDestroy(): void {
    this.ngUnsubscribe.next(null);
    this.ngUnsubscribe.complete();
  }
}
