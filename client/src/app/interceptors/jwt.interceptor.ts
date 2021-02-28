import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { AccountService } from '../Services/account.service';
import { Student } from '../models/student';
import { take } from 'rxjs/operators';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor(private accountService: AccountService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    let currentStudent: Student;

    this.accountService.currentStudent$.pipe(take(1)).subscribe(student => currentStudent = student);
    if(currentStudent) {
      request = request.clone(
        {
          setHeaders: {
            Authorization: 'Bearer ' + currentStudent.token
          }
        }
      )
    }

    return next.handle(request);
  }
}
