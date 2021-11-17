import { Component, OnInit } from '@angular/core';
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

  constructor(private adminService: AdminService,private toastr: ToastrService) { }

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
      });
  }

}
