import { HttpClient } from '@angular/common/http';
import { error } from '@angular/compiler/src/util';
import { Component, OnInit } from '@angular/core';
import { Student } from './models/student';
import { AccountService } from './Services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Student portal';
  students: any;

  constructor(private accountService: AccountService){}
  
  ngOnInit() {
    this.setCurrentStudent();
  }

  setCurrentStudent() {
    const student: Student = JSON.parse(localStorage.getItem('student'));
    this.accountService.setCurrentStudent(student);
  }

}
