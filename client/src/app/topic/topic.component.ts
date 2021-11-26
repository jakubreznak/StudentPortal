import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { CommentParams } from '../models/helpModels/commentParams';
import { Pagination } from '../models/helpModels/pagination';
import { Student } from '../models/student';
import { Topic, Comment } from '../models/topic';
import { AccountService } from '../Services/account.service';
import { DiskuzeService } from '../Services/diskuze.service';

@Component({
  selector: 'app-topic',
  templateUrl: './topic.component.html',
  styleUrls: ['./topic.component.scss']
})
export class TopicComponent implements OnInit {

  topic: Topic;
  text: string;
  student: Student;
  idIsEditing: number;
  textIsEditing: string;
  idReplyIsEditing: number;
  textReplyIsEditing: string;
  commentIdIsReplying: number;
  editForm: FormGroup;
  editReplyForm: FormGroup;
  replyForm: FormGroup;
  commentForm: FormGroup;
  pagination: Pagination;
  commentParams = new CommentParams();
  pagedComments: Comment[];
  filtersToggle: boolean = false;
  filtersToggleText: string = 'Filtry';
  commentsLiked: number[] = [];
  commentIdRepliesShown: number;

  constructor(private diskuzeService: DiskuzeService, private route: ActivatedRoute, private toastr: ToastrService, private accountService: AccountService) {
    this.accountService.currentStudent$.pipe(take(1)).subscribe(student => this.student = student);
   }

  ngOnInit(): void {
    this.initializeCommentForm();
    this.loadTopic();
    this.loadComments();
  }

  loadTopic(){
    this.diskuzeService.getTopic(Number(this.route.snapshot.paramMap.get('topicid'))).subscribe(response => {
      this.topic = response;
    });
  }

  loadComments(){
    this.diskuzeService.getComments(Number(this.route.snapshot.paramMap.get('topicid')), this.commentParams)
      .subscribe(response =>
        {
          this.pagedComments = response.result;
          this.pagination = response.pagination;
          
          this.accountService.getCurrentUserId().subscribe(id =>
            {
              let comments: number[] = [];
              response.result.forEach(function (comment){
                if(comment.studentsLikedBy.some(x => x.studentId == id))
                {                  
                  comments.push(comment.id);
                }
              })
              this.commentsLiked = comments;
            })          
        });
  }

  filter(){
    this.pagination.currentPage = 1;
    this.commentParams.pageNumber = 1;
    this.loadComments();
  }

  likeOrRemoveLike(commentId: number){
    if(this.commentsLiked.includes(commentId)){
      this.diskuzeService.removeLikeComment(commentId).subscribe(response =>{
        this.toastr.success("Komentář se vám již nelíbí.")
        this.loadComments();
      })
    }
    else if(!this.commentsLiked.includes(commentId))
    {
      this.diskuzeService.likeComment(commentId).subscribe(response =>{
        this.toastr.success("Líbí se vám daný materiál.")
        this.loadComments();
      })
    }
  }

  pageChanged(event: any){
    this.commentParams.pageNumber = event.page;
    this.loadComments();
    window.scrollTo(0, 0);
  }

  filterToggle(){
    this.filtersToggle = !this.filtersToggle;
    this.filtersToggleText = this.filtersToggle ? 'Skrýt filtry' : 'Filtry';
  }

  resetFilters(){
    this.commentParams = new CommentParams();
    this.pagination.currentPage = 1;
  }

  initializeCommentForm() {
    this.commentForm = new FormGroup({
      text: new FormControl('', [Validators.required, Validators.minLength(1)])
    })
  }

  postComment(){
    this.diskuzeService.postComment(this.topic.id, JSON.stringify(this.commentForm.value.text)).subscribe(topic =>
      {
        this.loadComments();
        this.toastr.success("Komentář přidán.");
        this.initializeCommentForm();
      });
  }

  postReply(commentId: number){
    this.diskuzeService.postReply(commentId, JSON.stringify(this.replyForm.value.text)).subscribe(reply =>
      {
        this.toastr.success("Odpověď přidána.");
        this.cancelReplying();
        this.replyForm.reset();
        this.loadComments();
      })
  }

  deleteReply(replyId: number){
    this.diskuzeService.deleteReply(replyId).subscribe(response => 
      {
        this.toastr.success("Opdověď smazána.");
        this.loadComments();
      })
  }

  deleteComment(topicID, commentID){
    this.diskuzeService.deleteComment(topicID, commentID).subscribe(comment =>
      {
        this.pagination.currentPage = 1;
        this.loadComments();
        this.toastr.success("Komentář odebrán.");
      });
  }

  initializeEditForm() {
    this.editForm = new FormGroup({
      text: new FormControl('',[Validators.required, Validators.minLength(1)])
    })
  }

  initializeEditReplyForm() {
    this.editReplyForm = new FormGroup({
      text: new FormControl('',[Validators.required, Validators.minLength(1)])
    })
  }

  editComment(commentID, commentText){
    this.textIsEditing = commentText;
    this.idIsEditing = commentID;
    this.initializeEditForm();
  }

  editReply(replyId, replyText){
    this.textReplyIsEditing = replyText;
    this.idReplyIsEditing = replyId;
    this.initializeEditReplyForm();
  }

  initializeReplyForm() {
    this.replyForm = new FormGroup({
      text: new FormControl('',[Validators.required, Validators.minLength(1)])
    })
  }

  reply(commentID){
    this.commentIdIsReplying = commentID;
    this.initializeReplyForm();
  }

  cancelReplying()
  {
    this.commentIdIsReplying = NaN;
  }

  saveEdit(topicID, commentID){
    this.diskuzeService.editComment(topicID, commentID, JSON.stringify(this.editForm.value.text)).subscribe(topic =>
    {
      this.topic = topic;
      this.toastr.success("Komentář byl úspěšně upraven.");
      this.cancelEdit();
      this.loadComments();
    });
  }

  cancelEdit()
  {
    this.idIsEditing = NaN;
  }

  saveEditReply(replyId: number){
    this.diskuzeService.editReply(replyId, JSON.stringify(this.editReplyForm.value.text)).subscribe(response =>
      {
        this.toastr.success("Odpověď upravena.");
        this.cancelReplyEdit();
        this.loadComments();
      })
  }

  cancelReplyEdit()
  {
    this.idReplyIsEditing = NaN;
  }

}
