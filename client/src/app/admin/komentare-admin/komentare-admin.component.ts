import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AdminService } from '../admin.service';

@Component({
  selector: 'app-komentare-admin',
  templateUrl: './komentare-admin.component.html',
  styleUrls: ['./komentare-admin.component.css']
})
export class KomentareAdminComponent implements OnInit {

  comments: Comment[];
  
  constructor(private adminService: AdminService,private toastr: ToastrService) { }

  ngOnInit(): void {
    this.getComments();
  }

  getComments() {
    this.adminService.getComments().subscribe(comments =>
      {
        this.comments = comments;
      })
  }

  deleteComment(id: number) {
    this.adminService.deleteComment(id).subscribe(r =>
      {
        this.comments = r;
        this.toastr.success("Komentář smazán.");
      }, error => {
        this.toastr.error(error.error);
      });
  }

}
