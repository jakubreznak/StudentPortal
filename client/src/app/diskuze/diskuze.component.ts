import { HttpClient } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { Predmet } from '../models/predmet';
import { Topic } from '../models/topic';
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

  constructor(private diskuzeService: DiskuzeService) { }

  ngOnInit(): void {
      this.loadTopics();
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

  
  postTopic(){
    if(this.predmet == null){
      this.diskuzeService.postTopic("x", JSON.stringify(this.topicName)).subscribe(topic =>
        this.topics.push(topic));
    }else{
      this.diskuzeService.postTopic(this.predmet.id.toString(), JSON.stringify(this.topicName)).subscribe(topic =>
        this.topics.push(topic));
    }
  }
}
