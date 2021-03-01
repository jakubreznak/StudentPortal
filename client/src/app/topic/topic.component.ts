import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Topic } from '../models/topic';
import { DiskuzeService } from '../Services/diskuze.service';

@Component({
  selector: 'app-topic',
  templateUrl: './topic.component.html',
  styleUrls: ['./topic.component.css']
})
export class TopicComponent implements OnInit {

  topic: Topic;
  text: string;

  constructor(private diskuzeService: DiskuzeService, private route: ActivatedRoute, private toastr: ToastrService) { }

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
  }

}
