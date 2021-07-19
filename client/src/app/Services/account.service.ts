import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ReplaySubject } from 'rxjs';
import {map} from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { PlanInfo, RootPlanInfo } from '../models/helpModels/planInfo';
import { PredmetOboru, RootPredmety } from '../models/helpModels/predmetOboru';
import { RegisterDTO } from '../models/helpModels/registerDTO';
import { RegisterForm } from '../models/helpModels/registerForm';
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
  plan : PlanInfo;
  predmety : PredmetOboru[];
  oborIdno : number;
  rocnikRegistrace : number;
  registerDTO: RegisterDTO = {name: "", password: "",upolNumber: "", oborIdno: 0, rocnikRegistrace: 0, predmety: [] };

  constructor(private router: Router, private http: HttpClient, private toastr: ToastrService) { }

  login(model: any) {
    return this.http.post(this.baseUrl + 'account/login', model).pipe(
      map((response: Student) => {
        const student = response;
        if(student)         
        this.setCurrentStudent(student);
      })
    )
  }

  register(model: RegisterForm){

    return this.http
    .get<RootPlanInfo>("https://stagservices.upol.cz/ws/services/rest2/programy/getPlanyStudenta?osCislo=" + model.upolNumber.toUpperCase().trim() + "&outputFormat=JSON")
    .pipe(map(response => 
      {
        if(response.planInfo[0] == null)
        {
          return 1;
        }
        this.plan = response.planInfo[0];
        this.rocnikRegistrace = Number(this.plan.nazev.replace(/[^0-9]/g, ''));
        this.http
        .get<RootPredmety>("https://stagservices.upol.cz/ws/services/rest2/predmety/getPredmetyByObor?oborIdno=" + this.plan.oborIdno + "&outputFormat=JSON")
        .subscribe(response =>
          {
            this.predmety = response.predmetOboru.filter((e, i) => i % 2 === 2 - 1);
            this.registerDTO.name = model.name;
            this.registerDTO.password = model.password;
            this.registerDTO.upolNumber = model.upolNumber;
            this.registerDTO.predmety = this.predmety;
            this.registerDTO.rocnikRegistrace = this.rocnikRegistrace;
            this.registerDTO.oborIdno = this.plan.oborIdno;

            return this.http.post(this.baseUrl + 'account/register', this.registerDTO).subscribe(
              (student: Student) => {
                if(student) {
                  this.setCurrentStudent(student);
                  this.router.navigateByUrl('/predmety');
                }
                else{
                  return 2;
                }
              }
            );
          });
      }));

    //   this.router.navigateByUrl('/predmety');
    // }, error => {
    //   this.toastr.error(error.error);
    // })
        
  }

  updateUpolNumber(upolNumber : string){
    return this.http.put(this.baseUrl + 'account', upolNumber, this.httpOptions);
  }

  deleteAccount(){
    return this.http.delete(this.baseUrl + 'account');
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
