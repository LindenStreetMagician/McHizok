import { ErrorHandler, Injectable, Injector } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { ErrorService } from './services/error.service';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class GlobalErrorHandler implements ErrorHandler {

    constructor(private injector: Injector) { }

    handleError(error: Error | HttpErrorResponse) {

        const errorService = this.injector.get(ErrorService);
        const toastr = this.injector.get(ToastrService);

        let message;

        if (error instanceof HttpErrorResponse) {
            message = errorService.getServerMessage(error).message;
        } else {
            message = errorService.getClientMessage(error);
        }

        toastr.error(message);
        console.error(error);
    }
}