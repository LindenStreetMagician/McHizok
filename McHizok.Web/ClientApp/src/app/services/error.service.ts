import { Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { ErrorDetails } from '../models/error-details.model';

@Injectable({
    providedIn: 'root'
})
export class ErrorService {

    getClientMessage(error: Error): string {
        if (!navigator.onLine) {
            return 'No Internet Connection';
        }

        return error.message ? error.message : error.toString();
    }

    getServerMessage(error: HttpErrorResponse): ErrorDetails {
        return error.error;
    }
}