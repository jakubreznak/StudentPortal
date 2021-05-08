import { HttpClient } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { Predmet } from '../models/predmet';
import { Student } from '../models/student';
import { Topic } from '../models/topic';
import { AccountService } from '../Services/account.service';
import { DiskuzeService } from '../Services/diskuze.service';

@Component({
  selector: 'app-diskuze',
  templateUrl: './diskuze.component.html',
  styleUrls: ['./diskuze.component.css']
})
export class DiskuzeComponent implements OnInit {
  @Input() predmet: Predmet;
  topics: Topic[];
  topicName: string;
  diskuzeForm: FormGroup;
  student: Student;

  constructor(private diskuzeService: DiskuzeService, private toastr: ToastrService, private accountService: AccountService) { 
    this.accountService.currentStudent$.pipe(take(1)).subscribe(student => this.student = student);
  }

  ngOnInit(): void {
      this.loadTopics();
      this.initializeForm();
  }

  initializeForm() {
    this.diskuzeForm = new FormGroup({
      topicName: new FormControl('', Validators.required)
    })
  }

  loadTopics() {
    if(this.predmet == null){
      this.diskuzeService.getTopicsByPredmetID("x").subscribe(topics =>
        this.topics = topics);
    }else{
      this.diskuzeService.getTopicsByPredmetID(this.predmet.id.toString()).subscribe(topics =>
        this.topics = topics);
    }
  }

  validateBeforeSubmit(){
    if(this.diskuzeForm.valid){
      this.postTopic();
    }else{
      this.diskuzeForm.markAllAsTouched();
    }
  }
  
  postTopic(){
    if(this.predmet == null){
      this.diskuzeService.postTopic("x", JSON.stringify(this.diskuzeForm.value.topicName)).subscribe(topic =>
        {
          this.topics.push(topic);
          this.toastr.success("Téma přidáno.");
        }, error => {
          this.toastr.error(error.error);
        });
    }else{
      this.diskuzeService.postTopic(this.predmet.id.toString(), JSON.stringify(this.diskuzeForm.value.topicName)).subscribe(topic =>
        {
          this.topics.push(topic);
          this.toastr.success("Téma přidáno.");
        }, error => {
          this.toastr.error(error.error);
        });
    }
    this.diskuzeForm.reset();
  }

  deleteTopic(topicID){
    this.diskuzeService.deleteTopic(topicID).subscribe(topic =>
      {
        this.toastr.success("Téma odebráno.");
        this.topics = this.topics.filter(t => t.id != topic.id);
      }, error => {
        this.toastr.error(error.error);
      });
  }
}
