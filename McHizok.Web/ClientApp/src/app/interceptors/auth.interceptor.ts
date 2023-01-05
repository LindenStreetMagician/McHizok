import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { ToastrService } from "ngx-toastr";
import { catchError, Observable } from "rxjs";
import { AuthService } from "../services/auth.service";

@Injectable({
    providedIn: 'root'
})
export class AuthInterceptor implements HttpInterceptor {
    constructor(private authService: AuthService, private router: Router, private toastr: ToastrService) { }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        var token = this.authService.getToken();

        if (token) {
            req = req.clone({
                setHeaders: {
                    Authorization: `Bearer ${token}`
                }
            });
        }

        return next.handle(req).pipe(
            catchError((error) => {
                if (error instanceof HttpErrorResponse && error.status === 401) {
                    this.authService.logout();
                    this.router.navigate(['login']);
                }
                throw error;
            })
        )
    }
}