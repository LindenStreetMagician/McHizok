import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ApplePieComponent } from './apple-pie/apple-pie.component';
import { LoginComponent } from './login/login.component';

const routes: Routes = [
  { path: '', component: ApplePieComponent },
  { path: 'login', component: LoginComponent },
  { path: '**', component: LoginComponent, pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
