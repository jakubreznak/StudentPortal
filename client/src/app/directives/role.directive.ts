import { Directive, Input, OnInit, TemplateRef, ViewContainerRef } from '@angular/core';
import { take } from 'rxjs/operators';
import { Student } from '../models/student';
import { AccountService } from '../Services/account.service';

@Directive({
  selector: '[appRole]'
})
export class RoleDirective implements OnInit{
  @Input() appRole: string[];
  student: Student;

  constructor(private viewContainerRef: ViewContainerRef, private templateRef: TemplateRef<any>,
    private accountService: AccountService) 
    {
      this.accountService.currentStudent$.pipe(take(1)).subscribe(student =>
        {
          this.student = student;
        })
    }

  ngOnInit(): void {
    if(!this.student?.roles || this.student == null) {
      this.viewContainerRef.clear();
      return;
    }

    if(this.student?.roles.some(r => this.appRole.includes(r))) {
      this.viewContainerRef.createEmbeddedView(this.templateRef);
    } else {
      this.viewContainerRef.clear();
    }
  }

}
