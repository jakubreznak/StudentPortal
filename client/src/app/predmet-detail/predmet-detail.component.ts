import { HttpHeaders } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Predmet } from '../models/predmet';
import { PredmetyService } from '../Services/predmety.service';

@Component({
  selector: 'app-predmet-detail',
  templateUrl: './predmet-detail.component.html',
  styleUrls: ['./predmet-detail.component.css']
})
export class PredmetDetailComponent implements OnInit {

  predmet: Predmet;
  predmetNazev: string;
  predmetId: number = Number(this.route.snapshot.paramMap.get('id'));
  pred: Predmet;

  constructor(private predmetService: PredmetyService, private route: ActivatedRoute, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.loadPredmet();
  }

  loadPredmet() {
    this.predmetService.getPredmet(this.predmetId).subscribe(predmet =>
      {
        this.predmet = predmet
      });
    this.predmetService.getPredmetName(this.predmetId).subscribe(predmet =>
      {
        this.pred = predmet;
        this.predmetNazev = predmet.nazev;
        console.log(this.pred.statut);
      });
      
  }

  removePredmet(){
    this.predmetService.removePredmet(this.route.snapshot.paramMap.get('id')).subscribe(response =>
      {
        this.toastr.success("Předmět odebrán ze seznamu.");
      });
  }

}
