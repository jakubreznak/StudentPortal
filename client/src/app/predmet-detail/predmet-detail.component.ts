import { Component, OnInit } from '@angular/core';
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

  constructor(private predmetService: PredmetyService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.loadPredmet();
  }

  loadPredmet() {
    this.predmetService.getPredmet(Number(this.route.snapshot.paramMap.get('id'))).subscribe(predmet =>
      this.predmet = predmet);
  }

}
