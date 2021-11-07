import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AdminTopicParams } from 'src/app/models/helpModels/adminTopicParams';
import { Pagination } from 'src/app/models/helpModels/pagination';
import { Topic } from 'src/app/models/topic';
import { AdminService } from '../admin.service';

@Component({
  selector: 'app-diskuze-admin',
  templateUrl: './diskuze-admin.component.html',
  styleUrls: ['./diskuze-admin.component.css']
})
export class DiskuzeAdminComponent implements OnInit {

  topics: Topic[];
  topicParams = new AdminTopicParams();
  pagination: Pagination;

  constructor(private adminService: AdminService,private toastr: ToastrService) { }

  ngOnInit(): void {
    this.getTopics();
  }

  
  getTopics() {
    this.adminService.getTopics(this.topicParams).subscribe(response =>
      {
        this.topics = response.result;
        this.pagination = response.pagination;
      })
  }

  resetFilters(){
    this.topicParams = new AdminTopicParams();
    this.pagination.currentPage = 1;
  }

  pageChanged(event: any) {
    this.topicParams.pageNumber = event.page;
    this.getTopics();
  }

  deleteTopic(id: number) {
    this.adminService.deleteTopic(id).subscribe(r =>
      {
        this.pagination.currentPage = 1;
        this.getTopics();
        this.toastr.success("Téma smazáno.");
      });
  }
}
