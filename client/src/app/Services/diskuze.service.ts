import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { CommentParams } from '../models/helpModels/commentParams';
import { PaginatedResult } from '../models/helpModels/pagination';
import { TopicParams } from '../models/helpModels/topicParams';
import { Student } from '../models/student';
import { Topic, Comment } from '../models/topic';
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

  getTopicsByPredmetID(predmetID: string, topicParams: TopicParams){
    let params = getPaginationHeaders(topicParams.pageNumber, topicParams.pageSize);

    params = params.append('Nazev', topicParams.nazev);
    params = params.append('Student', topicParams.student);

    return getPaginatedResult<Topic[]>(this.baseUrl + 'discussion/' + predmetID, params, this.http);
  }

  postTopic(predmetID: string, topicName: string){
    return this.http.
      post<Topic>(this.baseUrl + 'discussion/' + predmetID, topicName, this.httpOptions);
  }

  getComments(id: number, commentParams: CommentParams){
    let params = getPaginationHeaders(commentParams.pageNumber, commentParams.pageSize);

    params = params.append('Nazev', commentParams.nazev);
    params = params.append('Student', commentParams.student);

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

  likeComment(commentId: number){
    return this.http.post<number>(this.baseUrl + 'like/comment', commentId);
  }

  removeLikeComment(commentId: number){
    return this.http.delete<any>(this.baseUrl + 'like/comment/' + commentId);
  }
}
