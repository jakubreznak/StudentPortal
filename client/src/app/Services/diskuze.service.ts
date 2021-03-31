import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Student } from '../models/student';
import { Topic } from '../models/topic';
import { AccountService } from './account.service';

@Injectable({
  providedIn: 'root'
})
export class DiskuzeService {
  baseUrl = environment.apiUrl;
  student: Student;
  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type':  'application/json'
    })
  };

  constructor(private http: HttpClient, private accountService: AccountService) {
    this.accountService.currentStudent$.pipe(take(1)).subscribe(student => this.student = student);
   }

  getTopicsByPredmetID(predmetID: string){
    return this.http.get<Topic[]>(this.baseUrl + 'discussion/' + predmetID);
  }

  postTopic(predmetID: string, topicName: string){
    return this.http.
      post<Topic>(this.baseUrl + 'discussion/' + predmetID, topicName, this.httpOptions);
  }

  getTopic(id: number){
    return this.http.get<Topic>(this.baseUrl + 'discussion/topic/' + id);
  }

  postComment(topicId: number, text: string){
    return this.http.post<Topic>(this.baseUrl + 'discussion/comment/' + topicId, text, this.httpOptions);
  }

  deleteComment(topicID, commentID){
    return this.http.delete<Topic>(this.baseUrl + 'discussion/comment/' + topicID + '/' + commentID);
  }

  deleteTopic(topicID){
    return this.http.delete<Topic>(this.baseUrl + 'discussion/' + topicID);
  }
}
