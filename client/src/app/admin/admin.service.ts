import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { AdminCommentParams } from '../models/helpModels/adminCommentParams';
import { AdminHodnoceniParams } from '../models/helpModels/adminHodnoceniParams';
import { AdminMaterialParams } from '../models/helpModels/adminMaterialParams';
import { AdminTopicParams } from '../models/helpModels/adminTopicParams';
import { StudentsParams } from '../models/helpModels/studentsParams';
import { Hodnoceni, Soubor } from '../models/predmet';
import { Reply } from '../models/reply';
import { Student } from '../models/student';
import { Topic } from '../models/topic';
import { getPaginatedResult, getPaginationHeaders } from '../Services/paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getStudents(studentsParams: StudentsParams) {
    let params = getPaginationHeaders(studentsParams.pageNumber, studentsParams.pageSize);

    params = params.append('Nazev', studentsParams.nazev);

    return getPaginatedResult<string[]>(this.baseUrl + 'admin/students', params, this.http);
  }

  deleteStudent(name: string) {
    return this.http.delete<string[]>(this.baseUrl + 'admin/student/' + name);
  }

  getTopics(topicParams: AdminTopicParams) {
    let params = getPaginationHeaders(topicParams.pageNumber, topicParams.pageSize);

    params = params.append('Nazev', topicParams.nazev);
    params = params.append('Student', topicParams.student);

    return getPaginatedResult<Topic[]>(this.baseUrl + 'admin/topics', params, this.http);
  }

  deleteTopic(id: number) {
    return this.http.delete<Topic[]>(this.baseUrl + 'admin/topic/' + id);
  }

  getComments(commentParams: AdminCommentParams) {
    let params = getPaginationHeaders(commentParams.pageNumber, commentParams.pageSize);

    params = params.append('Nazev', commentParams.nazev);
    params = params.append('Student', commentParams.student);
    params = params.append('Tema', commentParams.tema);

    return getPaginatedResult<Comment[]>(this.baseUrl + 'admin/comments', params, this.http);
  }

  deleteComment(id: number) {
    return this.http.delete<Comment[]>(this.baseUrl + 'admin/comment/' + id);
  }

  deleteReply(id: number) {
    return this.http.delete<Reply>(this.baseUrl + 'admin/reply/' + id);
  }

  getHodnoceni(hodnoceniParams: AdminHodnoceniParams) {
    let params = getPaginationHeaders(hodnoceniParams.pageNumber, hodnoceniParams.pageSize);

    params = params.append('Text', hodnoceniParams.text);
    params = params.append('Student', hodnoceniParams.student);
    params = params.append('Predmet', hodnoceniParams.predmet);

    return getPaginatedResult<Hodnoceni[]>(this.baseUrl + 'admin/hodnoceni', params, this.http);
  }

  deleteHodnoceni(id: number) {
    return this.http.delete<Hodnoceni[]>(this.baseUrl + 'admin/hodnoceni/' + id);
  }

  getSoubory(materialParams: AdminMaterialParams) {
    let params = getPaginationHeaders(materialParams.pageNumber, materialParams.pageSize);

    params = params.append('Nazev', materialParams.nazev);
    params = params.append('Student', materialParams.student);
    params = params.append('Typ', materialParams.typ);

    return getPaginatedResult<Soubor[]>(this.baseUrl + 'admin/soubory', params, this.http);
  }

  deleteSoubor(id: number) {
    return this.http.delete<Soubor[]>(this.baseUrl + 'admin/soubor/' + id);
  }
}
