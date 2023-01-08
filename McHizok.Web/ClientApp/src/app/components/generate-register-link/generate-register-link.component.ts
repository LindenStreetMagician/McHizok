import { Component } from '@angular/core';
import { Clipboard } from '@angular/cdk/clipboard';
import { UserService } from 'src/app/services/user.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-generate-register-link',
  templateUrl: './generate-register-link.component.html',
  styleUrls: ['./generate-register-link.component.css']
})
export class GenerateRegisterLinkComponent {
  to: string = '';
  registrationToken: string = '';
  registrationUrl: string = '';
  constructor(private toastr: ToastrService, private userService: UserService, private clipboard: Clipboard) { }

  onClickGenerate() {
    if (this.to == "") {
      this.toastr.error('Missing for who!');
      return
    }

    this.userService.generateRegistrationLink(this.to).subscribe({
      next: (regToken) => {
        this.registrationToken = regToken;
        this.registrationUrl = `${location.origin}/register/${this.registrationToken}`;

        this.copyUrlToClipboard(this.registrationUrl);
      }
    });
  }

  reCopyUrl(tokenText: any) {
    if (tokenText.value == "") {
      return;
    }

    this.copyUrlToClipboard(this.registrationUrl);
  }

  private copyUrlToClipboard(url: string) {
    let copyResult = this.clipboard.copy(url);

    if (copyResult) {
      this.toastr.success('Link copied to clipboard', `${this.to} reg link`);
    } else {
      this.toastr.error('Copy failed.');
    }
  }
}
