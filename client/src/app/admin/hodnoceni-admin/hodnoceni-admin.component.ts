import { Component, OnInit, TemplateRef } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Pagination } from 'src/app/models/helpModels/pagination';
import { Hodnoceni } from 'src/app/models/predmet';
import { AdminService } from '../admin.service';
import { AdminHodnoceniParams } from 'src/app/models/helpModels/adminHodnoceniParams';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-hodnoceni-admin',
  templateUrl: './hodnoceni-admin.component.html',
  styleUrls: ['./hodnoceni-admin.component.scss']
})
export class HodnoceniAdminComponent implements OnInit {

  hodnoceni: Hodnoceni[];
  pagination: Pagination;
  showRating: number;
  hodnoceniParams = new AdminHodnoceniParams();
  modalRefRating?: BsModalRef;

  constructor(private adminService: AdminService,private toastr: ToastrService, private modalService: BsModalService) { }

  ngOnInit(): void {
    this.getHodnoceni();
  }

  getHodnoceni() {
    this.adminService.getHodnoceni(this.hodnoceniParams).subscribe(response =>
      {
        this.hodnoceni = response.result;
        this.pagination = response.pagination;
      })
  }

  openModalRating(template: TemplateRef<any>) {
    this.modalRefRating = this.modalService.show(template, {class: 'modal-sm'});
  }

  decline(): void {
    this.modalRefRating?.hide();
  }

  filter(){
    this.pagination.currentPage = 1;
    this.hodnoceniParams.pageNumber = 1;
    this.getHodnoceni();
  }

  deleteHodnoceni(id: number) {
    this.adminService.deleteHodnoceni(id).subscribe(r =>
      {
        this.pagination.currentPage = 1;
        this.getHodnoceni();
        this.toastr.success("Hodnocení smazáno.");
        this.modalRefRating?.hide();
      });
  }

  resetFilters(){
    this.hodnoceniParams = new AdminHodnoceniParams();
    this.pagination.currentPage = 1;
  }

  pageChanged(event: any) {
    this.hodnoceniParams.pageNumber = event.page;
    this.getHodnoceni();
  }

}
