import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../Services/account.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  profileForm: FormGroup;

  constructor(private accountService: AccountService, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.profileForm = new FormGroup({
      idNumber: new FormControl('', [Validators.required, Validators.pattern('^[a-zA-Z]{1}[+ 0-9]{5}$')])
    })
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
