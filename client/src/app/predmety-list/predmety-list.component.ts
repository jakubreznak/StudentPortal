import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-predmety-list',
  templateUrl: './predmety-list.component.html',
  styleUrls: ['./predmety-list.component.css']
})
export class PredmetyListComponent implements OnInit {

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
  }

}
