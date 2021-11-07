import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Pagination } from 'src/app/models/helpModels/pagination';
import { Hodnoceni } from 'src/app/models/predmet';
import { AdminService } from '../admin.service';
import { AdminHodnoceniParams } from 'src/app/models/helpModels/adminHodnoceniParams';

@Component({
  selector: 'app-hodnoceni-admin',
  templateUrl: './hodnoceni-admin.component.html',
  styleUrls: ['./hodnoceni-admin.component.css']
})
export class HodnoceniAdminComponent implements OnInit {

  hodnoceni: Hodnoceni[];
  pagination: Pagination;
  hodnoceniParams = new AdminHodnoceniParams();

  constructor(private adminService: AdminService,private toastr: ToastrService) { }

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

  deleteHodnoceni(id: number) {
    this.adminService.deleteHodnoceni(id).subscribe(r =>
      {
        this.pagination.currentPage = 1;
        this.getHodnoceni();
        this.toastr.success("Hodnocení smazáno.");
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
