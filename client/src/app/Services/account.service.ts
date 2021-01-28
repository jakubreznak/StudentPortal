import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import {map} from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Student } from '../models/student';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;
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

  getUser(name: string) {
    return this.http.get<User>(this.baseUrl + 'students/' + name);
  }
}
