import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { NgxSpinnerService } from 'ngx-spinner';
import { finalize, Observable } from "rxjs";

@Injectable({
    providedIn: 'root'
})
export class HttpLoadingInterceptor implements HttpInterceptor {
    spinnerName: string = "";

    constructor(private spinner: NgxSpinnerService) { }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        this.setSpinnerName(req);

        this.spinner.show(this.spinnerName);
        return next.handle(req).pipe(
            finalize(() => this.spinner.hide(this.spinnerName))
        );
    }

    private setSpinnerName(req: HttpRequest<any>) {
        if (req.url.includes('applepie')) {
            this.spinnerName = 'pie';
        }

        if (req.url.includes('authentication')) {
            this.spinnerName = 'auth'
        }
    }
}