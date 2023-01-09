import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { RegisterRequest } from 'src/app/models/register-request.model';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  public registerRequest: RegisterRequest = <RegisterRequest>{};
  public registrationToken: string = '';
  public repeatPassword: string = '';

  constructor(private toastr: ToastrService, private route: ActivatedRoute, private router: Router, private userService: UserService) {
    this.registerRequest.registrationToken = this.route.snapshot.params['regToken'];
  }

  register() {
    if (this.registerRequest.password !== this.repeatPassword) {
      this.toastr.error('The given passwords do not match.');
      return;
    }

    this.userService.registerUser(this.registerRequest).subscribe({
      next: () => {
        this.router.navigate(['/login']);
      },
    })
  }
}
