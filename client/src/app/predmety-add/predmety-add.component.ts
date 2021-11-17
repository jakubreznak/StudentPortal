import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { Pagination } from '../models/helpModels/pagination';
import { PredmetyAllParams } from '../models/helpModels/predmetyAllParams';
import { Predmet } from '../models/predmet';
import { Student } from '../models/student';
import { AccountService } from '../Services/account.service';
import { PredmetyService } from '../Services/predmety.service';

@Component({
  selector: 'app-predmety-add',
  templateUrl: './predmety-add.component.html',
  styleUrls: ['./predmety-add.component.scss']
})
export class PredmetyAddComponent implements OnInit {

  predmety: Predmet[];
  student: Student;
  predmetyParams = new PredmetyAllParams();
  pagination: Pagination;
  predmetyPridany: Predmet[] = [];

  constructor(private predmetService: PredmetyService, private accountService: AccountService, private toastr: ToastrService) { 
    this.accountService.currentStudent$.pipe(take(1)).subscribe(student => this.student = student);
  }

  ngOnInit(): void {
    this.loadPredmety();
  }

  loadPredmety() {
      this.predmetService.getPredmetyUniverzity(this.predmetyParams).subscribe(response =>
        {
          this.predmety = response.result
          this.pagination = response.pagination;
        });    
  }

  addPredmet(predmet: Predmet){
    this.predmetService.addPredmet(predmet).subscribe(response =>
      {
        this.predmetyPridany.push(predmet);
        this.toastr.success("Předmět byl přidán.");
      })
  }

  resetFilters(){
    this.predmetyParams = new PredmetyAllParams();
    this.pagination.currentPage = 1;
  }

  pageChanged(event: any) {
    this.predmetyParams.pageNumber = event.page;
    this.loadPredmety();
  }

}
