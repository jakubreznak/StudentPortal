import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Student } from 'src/app/models/student';
import { Topic } from 'src/app/models/topic';
import { AdminService } from '../admin.service';

@Component({
  selector: 'app-studenti-admin',
  templateUrl: './studenti-admin.component.html',
  styleUrls: ['./studenti-admin.component.css']
})
export class StudentiAdminComponent implements OnInit {

  studentNames: string[];

  constructor(private adminService: AdminService,private toastr: ToastrService) { }

  ngOnInit(): void {
    this.getStudents();
  }

  getStudents() {
    this.adminService.getStudents().subscribe(studenti =>
      {
        this.studentNames = studenti;
      })
  }

  deleteStudent(name: string) {
    this.adminService.deleteStudent(name).subscribe(r =>
      {
        this.studentNames = r;
        this.toastr.success("Student smazán.");
      }, error => {
        this.toastr.error(error.error);
      });
  }
}
