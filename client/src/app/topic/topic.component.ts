import { Component, OnInit } from '@angular/core';
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

  constructor(private diskuzeService: DiskuzeService, private route: ActivatedRoute, private toastr: ToastrService, private accountService: AccountService) {
    this.accountService.currentStudent$.pipe(take(1)).subscribe(student => this.student = student);
   }

  ngOnInit(): void {
    this.loadTopic();
  }

  loadTopic(){
    this.diskuzeService.getTopic(Number(this.route.snapshot.paramMap.get('topicid'))).subscribe(topic =>
      this.topic = topic);
  }

  postComment(){
    this.diskuzeService.postComment(this.topic.id, JSON.stringify(this.text)).subscribe(topic =>
      {
        this.topic = topic;
        this.toastr.success("Komentář přidán.");
      });
    this.text = "";
  }

  deleteComment(topicID, commentID){
    this.diskuzeService.deleteComment(topicID, commentID).subscribe(topic =>
      {
        this.topic = topic
        this.toastr.success("Komentář odebrán.");
      });
  }

}
