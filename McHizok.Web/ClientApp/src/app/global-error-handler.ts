import { ErrorHandler, Injectable, Injector } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { ErrorService } from './services/error.service';
import { ToastrService } from 'ngx-toastr';
import { MonitoringService } from './services/monitoring.service';

@Injectable()
export class GlobalErrorHandler implements ErrorHandler {

    constructor(private injector: Injector) { }

    handleError(error: Error | HttpErrorResponse) {

        const errorService = this.injector.get(ErrorService);
        const toastr = this.injector.get(ToastrService);
        const monitoringService = this.injector.get(MonitoringService);

        let message = 'An error has occured.';

        if (error instanceof HttpErrorResponse) {
            message = errorService.getServerMessage(error);
        } else {
            message = errorService.getClientMessage(error);
        }

        monitoringService.logException(error);
        toastr.error(message);
        console.error(error);
    }
}