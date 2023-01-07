import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { RegisterRequest } from 'src/app/models/register-request.model';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  public registerRequest: RegisterRequest = <RegisterRequest>{};
  public repeatPassword: string = '';

  constructor(private toastr: ToastrService) { }
  register() {
    if (this.registerRequest.password !== this.repeatPassword) {
      this.toastr.error('The given passwords do not match.');
      return;
    }
  }
}
