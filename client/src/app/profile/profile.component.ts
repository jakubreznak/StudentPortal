import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../Services/account.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  profileForm: FormGroup;

  constructor(private accountService: AccountService, private toastr: ToastrService, private router: Router) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.profileForm = new FormGroup({
      idNumber: new FormControl('', Validators.required)
    })
  }

  deleteAccount() {
    this.accountService.deleteAccount().subscribe(result =>
      {
        this.accountService.logout();
        this.router.navigateByUrl('/');
        this.toastr.success("Účet byl úspěšně smazán.");
      }, error => {
        this.toastr.error(error.error);
      });
  }

  validateBeforeSubmit(){
    if(this.profileForm.valid){
      this.updateUpolNumber();
    }else{
      this.profileForm.markAllAsTouched();
    }
  }

  updateUpolNumber(){
    this.accountService.updateUpolNumber(JSON.stringify(this.profileForm.value.idNumber)).subscribe(result =>
      {
        this.toastr.success("Profil upraven.");
      }, error => {
        this.toastr.error(error.error);
      });;
  }

}
