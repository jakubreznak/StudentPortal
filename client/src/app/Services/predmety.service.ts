import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Predmet } from '../models/predmet';

@Injectable({
  providedIn: 'root'
})
export class PredmetyService {
  baseUrl = environment.apiUrl;
  predmety: Predmet[] = [];

  constructor(private http: HttpClient) { }

  getPredmety() {
    return this.http.get<Predmet[]>(this.baseUrl + 'predmety');
  }

  getPredmetyByObor(idObor: number){
    if (this.predmety.length > 0) return of(this.predmety);
    return this.http.get<Predmet[]>(this.baseUrl + 'predmety/getbyobor/' + idObor).pipe(
      map(predmety =>
        {
          this.predmety = predmety;
          return predmety;
        })
    );
  }

  getPredmet(id: number) {
    return this.http.get<Predmet>(this.baseUrl + 'predmety/getbyid/' + id);
  }
}
