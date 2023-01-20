import { ErrorHandler, NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FormsModule } from '@angular/forms';
import { ApplePieComponent } from './components/apple-pie/apple-pie.component';
import { NgxSpinnerModule } from 'ngx-spinner';
import { HttpLoadingInterceptor } from './interceptors/http-loading.interceptor';
import { ToastrModule } from 'ngx-toastr';
import { GlobalErrorHandler } from './global-error-handler';
import { ClipboardModule } from '@angular/cdk/clipboard';
import { LoginComponent } from './components/login/login.component';
import { AuthInterceptor } from './interceptors/auth.interceptor';
import { RegisterComponent } from './components/register/register.component';
import { NavigationComponent } from './components/navigation/navigation.component';
import { GenerateRegisterLinkComponent } from './components/generate-register-link/generate-register-link.component';
import { UserManagementComponent } from './components/user-management/user-management.component';
import { CouponInventoryComponent } from './components/coupon-inventory/coupon-inventory.component';

@NgModule({
  declarations: [
    AppComponent,
    ApplePieComponent,
    LoginComponent,
    RegisterComponent,
    NavigationComponent,
    GenerateRegisterLinkComponent,
    UserManagementComponent,
    CouponInventoryComponent
  ],
  imports: [
    FormsModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-right'
    }),
    NgxSpinnerModule,
    ClipboardModule,
    HttpClientModule,
    AppRoutingModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: HttpLoadingInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    { provide: ErrorHandler, useClass: GlobalErrorHandler }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
