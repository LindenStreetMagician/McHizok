import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ApplePieComponent } from './components/apple-pie/apple-pie.component';
import { AuthGuard } from './guards/auth.guard';
import { LoginGuard } from './guards/login.guard';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { GenerateRegisterLinkComponent } from './components/generate-register-link/generate-register-link.component';
import { RegisterGuard } from './guards/register.guard';
import { UserManagementComponent } from './components/user-management/user-management.component';
import { AdminGuard } from './guards/admin.guard';
import { CouponInventoryComponent } from './components/coupon-inventory/coupon-inventory.component';

const routes: Routes = [
  { path: '', component: ApplePieComponent, canActivate: [AuthGuard] },
  { path: 'login', component: LoginComponent, canActivate: [LoginGuard] },
  { path: 'generate-register-link', component: GenerateRegisterLinkComponent, canActivate: [AdminGuard] },
  { path: 'register/:regToken', component: RegisterComponent, canActivate: [RegisterGuard] },
  { path: 'user-management', component: UserManagementComponent, canActivate: [AdminGuard] },
  { path: 'coupons', component: CouponInventoryComponent, canActivate: [AuthGuard] },
  { path: '**', component: LoginComponent, pathMatch: 'full', canActivate: [LoginGuard] }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
