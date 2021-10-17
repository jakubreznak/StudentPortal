import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
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

  constructor(private diskuzeService: DiskuzeService, private route: ActivatedRoute, private toastr: ToastrService, private accountService: AccountService) {
    this.accountService.currentStudent$.pipe(take(1)).subscribe(student => this.student = student);
   }

  ngOnInit(): void {
    this.initializeCommentForm();
    this.loadTopic();
  }

  loadTopic(){
    this.diskuzeService.getTopic(Number(this.route.snapshot.paramMap.get('topicid'))).subscribe(topic =>
      this.topic = topic);
  }

  initializeCommentForm() {
    this.commentForm = new FormGroup({
      text: new FormControl('', [Validators.required, Validators.minLength(1)])
    })
  }

  postComment(){
    this.diskuzeService.postComment(this.topic.id, JSON.stringify(this.commentForm.value.text)).subscribe(topic =>
      {
        this.topic = topic;
        this.toastr.success("Komentář přidán.");
        this.initializeCommentForm();
      });
  }

  deleteComment(topicID, commentID){
    this.diskuzeService.deleteComment(topicID, commentID).subscribe(topic =>
      {
        this.topic = topic
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
    });
  }

  cancelEdit()
  {
    this.idIsEditing = NaN;
  }

}
