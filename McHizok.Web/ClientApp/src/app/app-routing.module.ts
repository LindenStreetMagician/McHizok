import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ApplePieComponent } from './components/apple-pie/apple-pie.component';
import { AuthGuard } from './guards/auth.guard';
import { LoginGuard } from './guards/login.guard';
import { LoginComponent } from './components/login/login.component';

const routes: Routes = [
  { path: '', component: ApplePieComponent, canActivate: [AuthGuard] },
  { path: 'login', component: LoginComponent, canActivate: [LoginGuard] },
  { path: '**', component: LoginComponent, pathMatch: 'full', canActivate: [LoginGuard] }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
