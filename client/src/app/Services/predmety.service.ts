import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { PredmetyAllParams } from '../models/helpModels/predmetyAllParams';
import { PredmetyParams } from '../models/helpModels/predmetyParams';
import { Predmet, Soubor } from '../models/predmet';
import { Student } from '../models/student';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class PredmetyService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getPredmety() {
    return this.http.get<Predmet[]>(this.baseUrl + 'predmety');
  }

  getPredmetyStudenta(predmetyParams: PredmetyParams){
    let params = new HttpParams();

    params = params.append('Rocnik', predmetyParams.rocnik.toString());
    params = params.append('Statut', predmetyParams.statut);
    params = params.append('Nazev', predmetyParams.nazev);

    return this.http.get<Predmet[]>(this.baseUrl + 'predmety/student', { observe: 'body', params });
  }

  getPredmetyUniverzity(predmetyParams: PredmetyAllParams){
    let params = getPaginationHeaders(predmetyParams.pageNumber, predmetyParams.pageSize);

    params = params.append('Katedra', predmetyParams.katedra);
    params = params.append('Zkratka', predmetyParams.zkratka);
    params = params.append('Nazev', predmetyParams.nazev);

    return getPaginatedResult<Predmet[]>(this.baseUrl + 'predmety/all', params, this.http);
  }

  getPredmet(id: number) {
    return this.http.get<Predmet>(this.baseUrl + 'predmety/getbyid/' + id);
  }

  getPredmetName(id: number) {
    return this.http.get<Predmet>(this.baseUrl + 'predmety/getname/' + id);
  }

  deleteMaterial(predmetID, souborID){
    return this.http.delete<Soubor>(this.baseUrl + 'predmety/' + predmetID + '/' + souborID);
  }

  addPredmet(predmet: Predmet){
    return this.http.put<Predmet>(this.baseUrl + 'predmety/add', predmet);
  }

  removePredmet(predmetId){
    return this.http.delete<Student>(this.baseUrl + 'predmety/remove/' + predmetId);
  }
}
