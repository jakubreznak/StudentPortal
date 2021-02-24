import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Hodnoceni, Predmet } from '../models/predmet';
import { CommonModule } from '@angular/common';

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
  model: any = {};
  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type':  'application/json'
    })
  };

  constructor(private http: HttpClient) { 
    this.model.cislo = 'x'
  }

  ngOnInit(): void {
    this.loadHodnoceni();
  }

  rate(){
    this.http.post<Hodnoceni[]>(this.baseUrl + 'hodnoceni/' + this.predmet.id + '/' + this.model.cislo,
    JSON.stringify(this.model.text), this.httpOptions).subscribe(hodnoceni =>
      {
        this.hodnoceni = hodnoceni;
        this.cislo = this.getCislo(hodnoceni);
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
