import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { Pagination } from '../models/helpModels/pagination';
import { Student } from '../models/student';
import { Topic } from '../models/topic';
import { AccountService } from '../Services/account.service';
import { DiskuzeService } from '../Services/diskuze.service';

@Component({
  selector: 'app-topic',
  templateUrl: './topic.component.html',
  styleUrls: ['./topic.component.css']
})
export class TopicComponent implements OnInit {

  topic: Topic;
  text: string;
  student: Student;
  idIsEditing: number;
  textIsEditing: string;
  editForm: FormGroup;
  commentForm: FormGroup;
  pagination: Pagination;
  pageNumber = 1;
  pageSize = 10;
  pagedComments: Comment[];

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
    this.diskuzeService.getComments(Number(this.route.snapshot.paramMap.get('topicid')), this.pageNumber, this.pageSize).subscribe(response =>
    {
      this.pagedComments = response.result;
      this.pagination = response.pagination;        
    });
  }

  pageChanged(event: any){
    this.pageNumber = event.page;
    this.loadComments();
    window.scrollTo(0, 0);
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

  deleteComment(topicID, commentID){
    this.diskuzeService.deleteComment(topicID, commentID).subscribe(comment =>
      {
        this.pageNumber = 1;
        this.loadComments();
        this.pagination.currentPage = 1;
        this.toastr.success("Komentář odebrán.");
      });
  }

  initializeEditForm() {
    this.editForm = new FormGroup({
      text: new FormControl('',[Validators.required, Validators.minLength(1)])
    })
  }

  editComment(commentID, commentText){
    this.textIsEditing = commentText;
    this.idIsEditing = commentID;
    this.initializeEditForm();
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

}
