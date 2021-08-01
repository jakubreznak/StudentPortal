import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Topic } from 'src/app/models/topic';
import { AdminService } from '../admin.service';

@Component({
  selector: 'app-diskuze-admin',
  templateUrl: './diskuze-admin.component.html',
  styleUrls: ['./diskuze-admin.component.css']
})
export class DiskuzeAdminComponent implements OnInit {

  topics: Topic[];

  constructor(private adminService: AdminService,private toastr: ToastrService) { }

  ngOnInit(): void {
    this.getTopics();
  }

  
  getTopics() {
    this.adminService.getTopics().subscribe(topics =>
      {
        this.topics = topics;
      })
  }

  deleteTopic(id: number) {
    this.adminService.deleteTopic(id).subscribe(r =>
      {
        this.topics = r;
        this.toastr.success("Téma smazáno.");
      });
  }
}
