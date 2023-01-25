import { Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';

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

    getServerMessage(error: HttpErrorResponse) {
        if (error.status == 0) {
            return "The server is currently unavailable. Try again later.";
        }

        if (error.error?.message) {
            return error.error.message;
        }

        return error.message;
    }
}