import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../Services/account.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  registerForm: FormGroup;
  
  constructor(private accountService: AccountService, private router: Router, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.registerForm = new FormGroup({
      name: new FormControl('', Validators.required),
      password: new FormControl('', [Validators.required, Validators.pattern("^((?=.*[a-z])(?=.*?[0-9])).{1,}$"),
        Validators.minLength(6)]),
      confirmPassword: new FormControl('', [Validators.required, this.matchValues('password')]),
      upolNumber: new FormControl('')
    })
    this.registerForm.controls.password.valueChanges.subscribe(() => {
      this.registerForm.controls.confirmPassword.updateValueAndValidity();
    })
  }

  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control?.value === control?.parent?.controls[matchTo].value ? null : {isMatching: true}
    }
  }

  register() {
    if(this.registerForm.controls.upolNumber.value == "")
    {
      this.accountService.registerWOUpolNumber(this.registerForm.value);
    } else {
      this.accountService.register(this.registerForm.value).subscribe(response => 
        {
          if(response == 1)
          {
            this.toastr.error("K tomuto osobnímu číslu neexistuje žádný obor.");
          }
        });
    }
  }

  cancel() {
    this.cancelRegister.emit(false);
  }


}
