import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from '../Services/account.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
registerMode = false;

  constructor(public accountService: AccountService, private router: Router) { }

  ngOnInit(): void {
    if(this.accountService.currentStudent$ != null){
      this.router.navigateByUrl('/predmety');
    }
  }

  registerToggle(){
    this.registerMode = !this.registerMode;
  }

  cancelRegisterMode(event: boolean){
    this.registerMode = event;
  }
}
