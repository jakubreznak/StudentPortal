import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Hodnoceni, Predmet } from '../models/predmet';
import { CommonModule } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { take } from 'rxjs/operators';
import { Student } from '../models/student';
import { AccountService } from '../Services/account.service';

@Component({
  selector: 'app-hodnoceni',
  templateUrl: './hodnoceni.component.html',
  styleUrls: ['./hodnoceni.component.css']
})
export class HodnoceniComponent implements OnInit {
  @Input() predmet: Predmet;
  baseUrl = environment.apiUrl;
  hodnoceni: Hodnoceni[];
  cislo: number;
  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type':  'application/json'
    })
  };
  cisla: number[] = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
  hodnoceniForm: FormGroup;
  student: Student;

  constructor(private http: HttpClient, private toastr: ToastrService, private accountService: AccountService) { 
    this.accountService.currentStudent$.pipe(take(1)).subscribe(student => this.student = student);
  }

  ngOnInit(): void {
    this.loadHodnoceni();
    this.initializeForm();
  }

  initializeForm() {
    this.hodnoceniForm = new FormGroup({
      text: new FormControl(''),
      cislo: new FormControl('', Validators.required)
    })
  }

  validateBeforeSubmit(){
    if(this.hodnoceniForm.valid){
      this.rate();
    }else{
      this.hodnoceniForm.markAllAsTouched();
    }
  }

  rate(){
    this.http.post<Hodnoceni[]>(this.baseUrl + 'hodnoceni/' + this.predmet.id + '/' + this.hodnoceniForm.value.cislo,
    JSON.stringify(this.hodnoceniForm.value.text || ""), this.httpOptions).subscribe(hodnoceni =>
      {
        this.hodnoceni = hodnoceni;
        this.cislo = this.getCislo(hodnoceni);
        this.toastr.success("Hodnocení přidáno.");
      }, error => {
        this.toastr.error(error.error);
      });
    this.hodnoceniForm.reset();
  }

  loadHodnoceni(){
    
    this.http.get<Hodnoceni[]>(this.baseUrl + 'hodnoceni/' + this.predmet.id).subscribe(hodnoceni =>
      {
        this.hodnoceni = hodnoceni;
        this.cislo = this.getCislo(hodnoceni);
      });
  }

  deleteRating(hodnoceniID){
    this.http.delete<Hodnoceni>(this.baseUrl + 'hodnoceni/' + this.predmet.id + '/' + hodnoceniID).subscribe(hodnoceni =>
      {
        this.hodnoceni = this.hodnoceni.filter(h => h.id != hodnoceni.id);
        this.toastr.success("Hodnocení bylo úspěšně odebráno.");
      }
      );
  }

  getCislo(hodnoceni: Hodnoceni[]){
    let hodnota = 0;
    for(let i = 0; i < hodnoceni.length; i++){
      hodnota += hodnoceni[i].rating;
    }
    if(hodnoceni.length > 0){
      return Math.round(hodnota / hodnoceni.length);
    } else{
      return 0;
    }
  }

}
