import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { Student } from '../models/student';
import { AccountService } from '../Services/account.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  loginForm: FormGroup;

  constructor(public accountService: AccountService ,private router: Router, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.loginForm = new FormGroup({
      name: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required)
    })
  }

  validateBeforeSubmit(){
    if(this.loginForm.valid){
      this.login();
    }else{
      this.loginForm.markAllAsTouched();
    }
  }

  login() {
    this.accountService.login(this.loginForm.value).subscribe(response =>
      {
        this.router.navigateByUrl('/predmety');
      })
  }

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/');
  } 
}
