import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import {map} from 'rxjs/operators';
import { Student } from '../models/student';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = "https://localhost:5001/api/";
  private currentStudentSource = new ReplaySubject<Student>(1);
  currentStudent$ = this.currentStudentSource.asObservable();

  constructor(private http: HttpClient) { }

  login(model: any) {
    return this.http.post(this.baseUrl + 'account/login', model).pipe(
      map((response: Student) => {
        const student = response;
        if(student)
          localStorage.setItem('student', JSON.stringify(student));
          this.currentStudentSource.next(student);
      })
    )
  }

  register(model: any){
    return this.http.post(this.baseUrl + 'account/register', model).pipe(
      map((student: Student) => {
        if(student) {
          localStorage.setItem('student', JSON.stringify(student));
          this.currentStudentSource.next(student);
        }
      })
    )
  }

  setCurrentStudent(student: Student){
    this.currentStudentSource.next(student);
  }

  logout() {
    localStorage.removeItem('student');
    this.currentStudentSource.next(null);
  }
}
