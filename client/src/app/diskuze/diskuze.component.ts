import { HttpClient } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { Pagination } from '../models/helpModels/pagination';
import { TopicParams } from '../models/helpModels/topicParams';
import { Predmet } from '../models/predmet';
import { Student } from '../models/student';
import { Topic } from '../models/topic';
import { AccountService } from '../Services/account.service';
import { DiskuzeService } from '../Services/diskuze.service';

@Component({
  selector: 'app-diskuze',
  templateUrl: './diskuze.component.html',
  styleUrls: ['./diskuze.component.scss']
})
export class DiskuzeComponent implements OnInit {
  @Input() predmet: Predmet;
  topics: Topic[];
  pagination: Pagination;
  topicParams = new TopicParams();
  topicName: string;
  diskuzeForm: FormGroup;
  student: Student;
  predmetId = Number(this.route.snapshot.paramMap.get('id'));
  filtersToggle: boolean = false;
  filtersToggleText: string = 'Filtry';

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

  filterToggle(){
    this.filtersToggle = !this.filtersToggle;
    this.filtersToggleText = this.filtersToggle ? 'Skrýt filtry' : 'Filtry';
  }

  loadTopics() {
    if(this.predmet == null){
      this.diskuzeService.getTopicsByPredmetID("x", this.topicParams).subscribe(response =>
        {
          this.topics = response.result;
          this.pagination = response.pagination;
        });
    }else{
      this.diskuzeService.getTopicsByPredmetID(this.predmetId.toString(), this.topicParams).subscribe(response =>
        {
          this.topics = response.result;
          this.pagination = response.pagination;
        });
    }
  }

  filter(){
    this.pagination.currentPage = 1;
    this.topicParams.pageNumber = 1;
    this.loadTopics();
  }

  pageChanged(event: any){
    this.topicParams.pageNumber = event.page;
    this.loadTopics();
    window.scrollTo(0, 0);
  }

  resetFilters(){
    this.topicParams = new TopicParams();
    this.pagination.currentPage = 1;
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
          this.topicParams.pageNumber = 1;
          this.loadTopics();
          this.toastr.success("Téma přidáno.");
          this.diskuzeForm.reset();
        });
    }else{
      this.diskuzeService.postTopic(this.predmetId.toString(), JSON.stringify(this.diskuzeForm.value.topicName)).subscribe(topic =>
        {
          this.topicParams.pageNumber = 1;
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
        this.pagination.currentPage = 1;
        this.loadTopics();
      });
  }
}
