import { Component } from '@angular/core';
import { UserForAuth } from '../models/user-for-auth.model';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  public userForAuth: UserForAuth = new UserForAuth();

  login() {
    console.log(this.userForAuth);
  }
}
