import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { PredmetyParams } from '../models/helpModels/predmetyParams';
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

  getPredmetyByObor(idObor: number, predmetyParams: PredmetyParams){
    let params = new HttpParams();

    params = params.append('Rocnik', predmetyParams.rocnik.toString());
    params = params.append('Statut', predmetyParams.statut);
    params = params.append('Nazev', predmetyParams.nazev);

    return this.http.get<Predmet[]>(this.baseUrl + 'predmety/getbyobor/' + idObor, { observe: 'body', params });
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
}
