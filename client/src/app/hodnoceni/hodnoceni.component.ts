import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Hodnoceni, Predmet } from '../models/predmet';
import { CommonModule } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { FormControl, FormGroup, Validators } from '@angular/forms';

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

  constructor(private http: HttpClient, private toastr: ToastrService) { 
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
      });
  }

  loadHodnoceni(){
    
    this.http.get<Hodnoceni[]>(this.baseUrl + 'hodnoceni/' + this.predmet.id).subscribe(hodnoceni =>
      {
        this.hodnoceni = hodnoceni;
        this.cislo = this.getCislo(hodnoceni);
      });
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
