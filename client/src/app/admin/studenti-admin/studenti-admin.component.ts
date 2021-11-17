import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Pagination } from 'src/app/models/helpModels/pagination';
import { StudentsParams } from 'src/app/models/helpModels/studentsParams';
import { Student } from 'src/app/models/student';
import { Topic } from 'src/app/models/topic';
import { AdminService } from '../admin.service';

@Component({
  selector: 'app-studenti-admin',
  templateUrl: './studenti-admin.component.html',
  styleUrls: ['./studenti-admin.component.scss']
})
export class StudentiAdminComponent implements OnInit {

  studentNames: string[];
  studentsParams = new StudentsParams();
  pagination: Pagination;

  constructor(private adminService: AdminService,private toastr: ToastrService) { }

  ngOnInit(): void {
    this.getStudents();
  }

  getStudents() {
    this.adminService.getStudents(this.studentsParams).subscribe(response =>
      {
        this.studentNames = response.result;
        this.pagination = response.pagination;
      })
  }

  deleteStudent(name: string) {
    this.adminService.deleteStudent(name).subscribe(r =>
      {
        this.pagination.currentPage = 1;
        this.getStudents();
        this.toastr.success("Student smazán.");
      });
  }

  resetFilters(){
    this.studentsParams = new StudentsParams();
    this.pagination.currentPage = 1;
  }

  pageChanged(event: any) {
    this.studentsParams.pageNumber = event.page;
    this.getStudents();
  }
}
