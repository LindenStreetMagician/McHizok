import { Component, OnDestroy, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Subject, takeUntil } from 'rxjs';
import { User } from 'src/app/models/user.model';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.css']
})
export class UserManagementComponent implements OnInit, OnDestroy {
  Users: User[] = [];
  private ngUnsubscribe = new Subject;

  constructor(private userService: UserService, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.userService.getUsers().pipe(takeUntil(this.ngUnsubscribe)).subscribe({
      next: (users) => {
        this.Users = users;
      }
    });
  }

  onClickDelete(user: User) {
    if (window.confirm(`Delete ${user.userName}? (made for: ${user.accountFor})`)) {
      this.userService.deleteUser(user.userId).pipe(takeUntil(this.ngUnsubscribe)).subscribe({
        next: () => {
          let index = this.Users.indexOf(user);
          this.Users.splice(index, 1);

          this.toastr.success(`${user.userName} was deleted successfully.`);
        }
      });
    }
  }

  ngOnDestroy(): void {
    this.ngUnsubscribe.next(null);
    this.ngUnsubscribe.complete();
  }
}
