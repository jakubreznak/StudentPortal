import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Hodnoceni } from 'src/app/models/predmet';
import { AdminService } from '../admin.service';

@Component({
  selector: 'app-hodnoceni-admin',
  templateUrl: './hodnoceni-admin.component.html',
  styleUrls: ['./hodnoceni-admin.component.css']
})
export class HodnoceniAdminComponent implements OnInit {

  hodnoceni: Hodnoceni[];

  constructor(private adminService: AdminService,private toastr: ToastrService) { }

  ngOnInit(): void {
    this.getHodnoceni();
  }

  getHodnoceni() {
    this.adminService.getHodnoceni().subscribe(hodnoceni =>
      {
        this.hodnoceni = hodnoceni;
      })
  }

  deleteHodnoceni(id: number) {
    this.adminService.deleteHodnoceni(id).subscribe(r =>
      {
        this.hodnoceni = r;
        this.toastr.success("Hodnocení smazáno.");
      });
  }

}
