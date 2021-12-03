import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { AdminMaterialParams } from 'src/app/models/helpModels/adminMaterialParams';
import { Pagination } from 'src/app/models/helpModels/pagination';
import { Soubor } from 'src/app/models/predmet';
import { AdminService } from '../admin.service';

@Component({
  selector: 'app-materialy-admin',
  templateUrl: './materialy-admin.component.html',
  styleUrls: ['./materialy-admin.component.scss']
})
export class MaterialyAdminComponent implements OnInit {

  soubory: Soubor[];
  pagination: Pagination;
  materialParams = new AdminMaterialParams();
  modalRefMaterial?: BsModalRef;

  constructor(private adminService: AdminService,private toastr: ToastrService, private modalService: BsModalService) { }

  ngOnInit(): void {
    this.getSoubory();
  }

  getSoubory() {
    this.adminService.getSoubory(this.materialParams).subscribe(response =>
      {
        this.soubory = response.result;
        this.pagination = response.pagination;
      })
  }

  openModalMaterial(template: TemplateRef<any>) {
    this.modalRefMaterial = this.modalService.show(template, {class: 'modal-sm'});
  }

  decline(): void {
    this.modalRefMaterial?.hide();
  }

  filter(){
    this.pagination.currentPage = 1;
    this.materialParams.pageNumber = 1;
    this.getSoubory();
  }

  resetFilters(){
    this.materialParams = new AdminMaterialParams();
    this.pagination.currentPage = 1;
  }

  pageChanged(event: any) {
    this.materialParams.pageNumber = event.page;
    this.getSoubory();
  }


  deleteSoubor(id: number) {
    this.adminService.deleteSoubor(id).subscribe(r =>
      {
        this.pagination.currentPage = 1;
        this.getSoubory();
        this.toastr.success("Soubor smazán.");
        this.modalRefMaterial?.hide();
      });
  }

}
