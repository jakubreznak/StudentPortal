import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { take } from 'rxjs/operators';
import { PredmetyParams } from '../models/helpModels/predmetyParams';
import { Predmet } from '../models/predmet';
import { Student } from '../models/student';
import { User } from '../models/user';
import { AccountService } from '../Services/account.service';
import { PredmetyService } from '../Services/predmety.service';

@Component({
  selector: 'app-predmety-list',
  templateUrl: './predmety-list.component.html',
  styleUrls: ['./predmety-list.component.css']
})
export class PredmetyListComponent implements OnInit {

  predmety: Predmet[];
  student: Student;
  predmetyTotalCount: number;
  predmetyParams = new PredmetyParams();
  statutList = [{value: 'all', display: 'Všechny'}, {value: 'A', display: 'A'}, {value: 'B', display: 'B'}, {value: 'C', display: 'C'}, {value: 'bez', display: 'Bez statutu'}];
  rocnikList = [{value: 0, display: 'Všechny'}, {value: 1, display: '1'}, {value: 2, display: '2'}, {value: 3, display: '3'}, {value: 4, display: 'Bez ročníku'}];

  constructor(private predmetService: PredmetyService, private accountService: AccountService) { 
    this.accountService.currentStudent$.pipe(take(1)).subscribe(student => this.student = student);
  }

  ngOnInit(): void {
    this.loadPredmety();
  }

  loadPredmety() {

    this.predmetService.getPredmetyStudenta(this.predmetyParams).subscribe(predmety =>
      this.predmety = predmety);    

    this.predmetService.predmetyStudentaCount().subscribe(response =>
      this.predmetyTotalCount = response);
  }

  resetFilters(){
    this.predmetyParams = new PredmetyParams();
  }

  getBackgroundColor(predmetStatut: string){
    switch (predmetStatut) {
      case 'A':
        return '#de5269';
      case 'B':
        return '#6093C0';
      case 'C':
        return '#67e5ab';
    }
  }

}
