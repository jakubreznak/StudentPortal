import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Predmet, Soubor } from '../models/predmet';

@Injectable({
  providedIn: 'root'
})
export class PredmetyService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getPredmety() {
    return this.http.get<Predmet[]>(this.baseUrl + 'predmety');
  }

  getPredmetyByObor(idObor: number){
    return this.http.get<Predmet[]>(this.baseUrl + 'predmety/getbyobor/' + idObor);
  }

  getPredmet(id: number) {
    return this.http.get<Predmet>(this.baseUrl + 'predmety/getbyid/' + id);
  }

  deleteMaterial(predmetID, souborID){
    return this.http.delete<Soubor>(this.baseUrl + 'predmety/' + predmetID + '/' + souborID);
  }
}
