import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private toastr: ToastrService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    
    if(request.url.includes("stagservices.upol.cz"))
    {
      return next.handle(request).pipe(
        catchError(error => {
          if (error) {
            this.toastr.error("Zkontrolujte že se nacházíte ve vnitřní síti a máte vypnuté CORS.");
          }
          return throwError(error);
        })
      );
    }
    else
    {
      return next.handle(request).pipe(
        catchError(error => {
          if (error) {
            this.toastr.error(error.error);
          }
          return throwError(error);
        })
      );
    }
  }
}
