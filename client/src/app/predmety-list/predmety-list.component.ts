import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { take } from 'rxjs/operators';
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

  constructor(private predmetService: PredmetyService, private accountService: AccountService) { 
    this.accountService.currentStudent$.pipe(take(1)).subscribe(student => this.student = student);
  }

  ngOnInit(): void {
    this.loadPredmety();
  }

  loadPredmety() {
    this.accountService.getOborIdByUsername(this.student.name).subscribe(oborId =>
      this.predmetService.getPredmetyByObor(oborId).subscribe(predmety =>
        this.predmety = predmety));
    
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
