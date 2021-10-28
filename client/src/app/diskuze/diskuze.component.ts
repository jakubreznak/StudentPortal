import { HttpClient } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { Pagination } from '../models/helpModels/pagination';
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
  pagination: Pagination;
  pageNumber = 1;
  pageSize = 10;
  topicName: string;
  diskuzeForm: FormGroup;
  student: Student;
  predmetId = Number(this.route.snapshot.paramMap.get('id'));

  constructor(private diskuzeService: DiskuzeService, private toastr: ToastrService, private accountService: AccountService,
    private route: ActivatedRoute) { 
    this.accountService.currentStudent$.pipe(take(1)).subscribe(student => this.student = student);
  }

  ngOnInit(): void {
      this.loadTopics();
      this.initializeForm();
  }

  initializeForm() {
    this.diskuzeForm = new FormGroup({
      topicName: new FormControl('', [Validators.required, Validators.maxLength(200)])
    })
  }

  loadTopics() {
    if(this.predmet == null){
      this.diskuzeService.getTopicsByPredmetID("x", this.pageNumber, this.pageSize).subscribe(response =>
        {
          this.topics = response.result;
          this.pagination = response.pagination;
        });
    }else{
      this.diskuzeService.getTopicsByPredmetID(this.predmetId.toString(), this.pageNumber, this.pageSize).subscribe(response =>
        {
          this.topics = response.result;
          this.pagination = response.pagination;
        });
    }
  }

  pageChanged(event: any){
    this.pageNumber = event.page;
    this.loadTopics();
    window.scrollTo(0, 0);
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
          this.pageNumber = 1;
          this.loadTopics();
          this.toastr.success("Téma přidáno.");
          this.diskuzeForm.reset();
        });
    }else{
      this.diskuzeService.postTopic(this.predmetId.toString(), JSON.stringify(this.diskuzeForm.value.topicName)).subscribe(topic =>
        {
          this.pageNumber = 1;
          this.loadTopics();
          this.toastr.success("Téma přidáno.");
          this.diskuzeForm.reset();
        });
    }    
  }

  deleteTopic(topicID){
    this.diskuzeService.deleteTopic(topicID).subscribe(topic =>
      {
        this.toastr.success("Téma odebráno.");
        this.pageNumber = 1;
        this.loadTopics();
        this.pagination.currentPage = 1;
      });
  }
}
