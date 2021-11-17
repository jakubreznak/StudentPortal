import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../Services/account.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {

  profileForm: FormGroup;
  changePasswordForm: FormGroup;

  constructor(private accountService: AccountService, private toastr: ToastrService, private router: Router) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.changePasswordForm = new FormGroup({
      oldPassword: new FormControl('', Validators.required),
      newPassword: new FormControl('', [Validators.required, Validators.pattern("^((?=.*[a-z])(?=.*?[0-9])).{1,}$"),
        Validators.minLength(6)]),
      confirmNewPassword: new FormControl('', [Validators.required, this.matchValues('newPassword')])
    })
    this.changePasswordForm.controls.newPassword.valueChanges.subscribe(() => {
      this.changePasswordForm.controls.confirmNewPassword.updateValueAndValidity();
    })

    this.profileForm = new FormGroup({
      idNumber: new FormControl('', Validators.required)
    })
  }

  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control?.value === control?.parent?.controls[matchTo].value ? null : {isMatching: true}
    }
  }

  deleteAccount() {
    this.accountService.deleteAccount().subscribe(result =>
      {
        this.accountService.logout();
        this.router.navigateByUrl('/');
        this.toastr.success("Účet byl úspěšně smazán.");
      });
  }

  changePassword() {
    this.accountService.changePassword(this.changePasswordForm.value).subscribe(result =>
      {
        this.toastr.success("Heslo bylo úspěšně změněno.");
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
    this.accountService.updateUpolNumber(this.profileForm.value.idNumber).subscribe(result =>
      {
        
          if(result == 1)
          {
            this.toastr.error("K tomuto osobnímu číslu neexistuje žádný obor.");
          }else{
            this.toastr.success("Profil upraven.");
          }
      })
  }

}
