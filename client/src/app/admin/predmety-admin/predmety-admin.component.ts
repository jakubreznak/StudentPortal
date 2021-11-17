import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { PredmetKatedry, PredmetyKatedry } from 'src/app/models/helpModels/predmetInfo';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-predmety-admin',
  templateUrl: './predmety-admin.component.html',
  styleUrls: ['./predmety-admin.component.scss']
})
export class PredmetyAdminComponent implements OnInit {

  upolBaseUrl = environment.upolApiUrl;
  baseUrl = environment.apiUrl;
  predmety: PredmetKatedry[];
  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type':  'application/json'
    })
  };

  constructor(private http: HttpClient, private toastr: ToastrService) { }

  ngOnInit(): void {
    
  }

  updatePredmety(){
    this.http.get<PredmetyKatedry>(this.upolBaseUrl + "/services/rest2/predmety/getPredmetyByKatedra?outputFormat=JSON") //&katedra=KMI
          .subscribe(response =>
      {
        return this.http.put(this.baseUrl + 'predmety', response.predmetKatedry, this.httpOptions).subscribe(response =>
          {
            this.toastr.success("Přeměty byli úspěšně aktualizovány.");
          });
      });
  }

}
