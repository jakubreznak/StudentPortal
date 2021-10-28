import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { PaginatedResult } from '../models/helpModels/pagination';
import { Student } from '../models/student';
import { Topic } from '../models/topic';
import { AccountService } from './account.service';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';

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
  paginatedResult: PaginatedResult<Topic[]> = new PaginatedResult<Topic[]>();

  constructor(private http: HttpClient, private accountService: AccountService) {
    this.accountService.currentStudent$.pipe(take(1)).subscribe(student => this.student = student);
   }

  getTopicsByPredmetID(predmetID: string, page?: number, itemsPerPage?: number){
    let params = getPaginationHeaders(page, itemsPerPage);

    return getPaginatedResult<Topic[]>(this.baseUrl + 'discussion/' + predmetID, params, this.http);
  }

  postTopic(predmetID: string, topicName: string){
    return this.http.
      post<Topic>(this.baseUrl + 'discussion/' + predmetID, topicName, this.httpOptions);
  }

  getComments(id: number, page?: number, itemsPerPage?: number){
    let params = getPaginationHeaders(page, itemsPerPage);

    return getPaginatedResult<Comment[]>(this.baseUrl + 'discussion/topic/' + id, params, this.http);
  }

  getTopic(id: number){
    return this.http.get<Topic>(this.baseUrl + 'discussion/topicInfo/' + id);
  }

  postComment(topicId: number, text: string){
    return this.http.post<Comment>(this.baseUrl + 'discussion/comment/' + topicId, text, this.httpOptions);
  }

  deleteComment(topicID, commentID){
    return this.http.delete<Comment>(this.baseUrl + 'discussion/comment/' + topicID + '/' + commentID);
  }

  deleteTopic(topicID){
    return this.http.delete<Topic>(this.baseUrl + 'discussion/' + topicID);
  }

  editComment(topicID: number, commentID: number, text: string){
  return this.http.put<Topic>(this.baseUrl + 'discussion/comment/' + topicID + '/' + commentID, text, this.httpOptions);
}
}
