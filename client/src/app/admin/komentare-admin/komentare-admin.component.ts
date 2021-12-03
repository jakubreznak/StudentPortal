import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { AdminCommentParams } from 'src/app/models/helpModels/adminCommentParams';
import { Pagination } from 'src/app/models/helpModels/pagination';
import { AdminService } from '../admin.service';

@Component({
  selector: 'app-komentare-admin',
  templateUrl: './komentare-admin.component.html',
  styleUrls: ['./komentare-admin.component.scss']
})
export class KomentareAdminComponent implements OnInit {

  comments: Comment[];
  pagination: Pagination;
  commentParams = new AdminCommentParams();
  commentIdRepliesShown: number;
  showComment: number;
  modalRefComment?: BsModalRef;
  modalRefReply?: BsModalRef;
  
  constructor(private adminService: AdminService,private toastr: ToastrService, private modalService: BsModalService) { }

  ngOnInit(): void {
    this.getComments();
  }

  getComments() {
    this.adminService.getComments(this.commentParams).subscribe(response =>
      {
        this.comments = response.result;
        this.pagination = response.pagination;
      })
  }

  openModalComment(template: TemplateRef<any>) {
    this.modalRefComment = this.modalService.show(template, {class: 'modal-sm'});
  }

  decline(): void {
    this.modalRefComment?.hide();
  }

  openModalReply(replyModal: TemplateRef<any>) {
    this.modalRefReply = this.modalService.show(replyModal, {class: 'modal-sm'});
  }

  declineReplyDelete(): void {
    this.modalRefReply?.hide();
  }

  filter(){
    this.pagination.currentPage = 1;
    this.commentParams.pageNumber = 1;
    this.getComments();
  }

  resetFilters(){
    this.commentParams = new AdminCommentParams();
    this.pagination.currentPage = 1;
  }

  pageChanged(event: any) {
    this.commentParams.pageNumber = event.page;
    this.getComments();
  }

  deleteComment(id: number) {
    this.adminService.deleteComment(id).subscribe(r =>
      {
        this.pagination.currentPage = 1;
        this.getComments();
        this.toastr.success("Komentář smazán.");
        this.modalRefComment?.hide();
      });
  }

  
  deleteReply(replyId: number){
    this.adminService.deleteReply(replyId).subscribe(response => 
      {
        this.toastr.success("Opdověď smazána.");
        this.getComments();
        this.declineReplyDelete();
      })
  }

}
