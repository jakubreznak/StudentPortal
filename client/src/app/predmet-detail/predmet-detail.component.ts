import { HttpHeaders } from '@angular/common/http';
import { Component, Input, OnInit, TemplateRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { Predmet } from '../models/predmet';
import { PredmetyService } from '../Services/predmety.service';

@Component({
  selector: 'app-predmet-detail',
  templateUrl: './predmet-detail.component.html',
  styleUrls: ['./predmet-detail.component.scss']
})
export class PredmetDetailComponent implements OnInit {

  predmet: Predmet;
  predmetNazev: string;
  predmetId: number = Number(this.route.snapshot.paramMap.get('id'));
  pred: Predmet;
  modalRefPredmet?: BsModalRef;

  constructor(private predmetService: PredmetyService, private route: ActivatedRoute,
     private toastr: ToastrService, private router: Router, private modalService: BsModalService) { }

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
      });
      
  }

  openModalPredmet(template: TemplateRef<any>) {
    this.modalRefPredmet = this.modalService.show(template, {class: 'modal-sm'});
  }

  decline(): void {
    this.modalRefPredmet?.hide();
  }

  removePredmet(){
    this.predmetService.removePredmet(this.route.snapshot.paramMap.get('id')).subscribe(response =>
      {
        this.toastr.success("Předmět odebrán ze seznamu.");
        this.router.navigate([".."]);
        this.modalRefPredmet?.hide();
      });
  }

}
