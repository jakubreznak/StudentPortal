import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import {map} from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Student } from '../models/student';


@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;
  private currentStudentSource = new ReplaySubject<Student>(1);
  currentStudent$ = this.currentStudentSource.asObservable();
  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type':  'application/json'
    })
  };

  constructor(private http: HttpClient) { }

  login(model: any) {
    return this.http.post(this.baseUrl + 'account/login', model).pipe(
      map((response: Student) => {
        const student = response;
        if(student)         
        this.setCurrentStudent(student);
      })
    )
  }

  register(model: any){
    return this.http.post(this.baseUrl + 'account/register', model).pipe(
      map((student: Student) => {
        if(student) {
          this.setCurrentStudent(student);
        }
      })
    )
  }

  updateUpolNumber(upolNumber : string){
    return this.http.put(this.baseUrl + 'account', upolNumber, this.httpOptions);
  }

  setCurrentStudent(student: Student){
    student.roles = [];
    const roles = this.getDecodedToken(student.token).role;
    Array.isArray(roles) ? student.roles = roles : student.roles.push(roles);
    localStorage.setItem('student', JSON.stringify(student));
    this.currentStudentSource.next(student);
  }

  logout() {
    localStorage.removeItem('student');
    this.currentStudentSource.next(null);
  }

  getOborIdByUsername(name: string) {
    return this.http.get<number>(this.baseUrl + 'students/' + name);
  }

  getDecodedToken(token){
    return JSON.parse(atob(token.split('.')[1]));
  }
}
