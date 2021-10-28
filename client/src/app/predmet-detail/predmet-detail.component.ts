import { HttpHeaders } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
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

  constructor(private predmetService: PredmetyService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.loadPredmet();
  }

  loadPredmet() {
    this.predmetService.getPredmet(this.predmetId).subscribe(predmet =>
      this.predmet = predmet);
    this.predmetService.getPredmetName(this.predmetId).subscribe(predmet =>
      {
        this.predmetNazev = predmet.nazev;
      });
  }

}
