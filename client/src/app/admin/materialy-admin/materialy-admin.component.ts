import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Soubor } from 'src/app/models/predmet';
import { AdminService } from '../admin.service';

@Component({
  selector: 'app-materialy-admin',
  templateUrl: './materialy-admin.component.html',
  styleUrls: ['./materialy-admin.component.css']
})
export class MaterialyAdminComponent implements OnInit {

  soubory: Soubor[];

  constructor(private adminService: AdminService,private toastr: ToastrService) { }

  ngOnInit(): void {
    this.getSoubory();
  }

  getSoubory() {
    this.adminService.getSoubory().subscribe(soubory =>
      {
        this.soubory = soubory;
      })
  }

  deleteSoubor(id: number) {
    this.adminService.deleteSoubor(id).subscribe(r =>
      {
        this.soubory = r;
        this.toastr.success("Soubor smazán.");
      });
  }

}
