import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
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
  modalRefStudent?: BsModalRef;

  constructor(private adminService: AdminService,private toastr: ToastrService, private modalService: BsModalService) { }

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

  openModalStudent(template: TemplateRef<any>) {
    this.modalRefStudent = this.modalService.show(template, {class: 'modal-sm'});
  }

  decline(): void {
    this.modalRefStudent?.hide();
  }

  filter(){
    this.pagination.currentPage = 1;
    this.studentsParams.pageNumber = 1;
    this.getStudents();
  }

  deleteStudent(name: string) {
    this.adminService.deleteStudent(name).subscribe(r =>
      {
        this.pagination.currentPage = 1;
        this.getStudents();
        this.toastr.success("Student smazán.");
        this.modalRefStudent?.hide();
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
