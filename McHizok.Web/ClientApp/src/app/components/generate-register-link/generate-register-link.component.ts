import { Component, OnDestroy } from '@angular/core';
import { Clipboard } from '@angular/cdk/clipboard';
import { UserService } from 'src/app/services/user.service';
import { ToastrService } from 'ngx-toastr';
import { Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-generate-register-link',
  templateUrl: './generate-register-link.component.html',
  styleUrls: ['./generate-register-link.component.css']
})
export class GenerateRegisterLinkComponent implements OnDestroy {
  accountFor: string = '';
  registrationToken: string = '';
  registrationUrl: string = '';
  private ngUnsubscribe = new Subject;

  constructor(private toastr: ToastrService,
    private userService: UserService,
    private clipboard: Clipboard) { }

  onClickGenerate() {
    if (this.accountFor == "") {
      this.toastr.error('Missing for who!');
      return
    }

    this.userService.generateRegistrationLink(this.accountFor).pipe(takeUntil(this.ngUnsubscribe)).subscribe({
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
      this.toastr.success('Link copied to clipboard', `${this.accountFor} reg link`);
    } else {
      this.toastr.error('Copy failed.');
    }
  }

  ngOnDestroy(): void {
    this.ngUnsubscribe.next(null);
    this.ngUnsubscribe.complete();
  }
}
