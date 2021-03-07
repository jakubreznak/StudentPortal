import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Hodnoceni, Soubor } from '../models/predmet';
import { Student } from '../models/student';
import { Topic } from '../models/topic';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getStudents() {
    return this.http.get<string[]>(this.baseUrl + 'admin/students')
  }

  deleteStudent(name: string) {
    return this.http.delete<string[]>(this.baseUrl + 'admin/student/' + name);
  }

  getTopics() {
    return this.http.get<Topic[]>(this.baseUrl + 'admin/topics')
  }

  deleteTopic(id: number) {
    return this.http.delete<Topic[]>(this.baseUrl + 'admin/topic/' + id);
  }

  getComments() {
    return this.http.get<Comment[]>(this.baseUrl + 'admin/comments')
  }

  deleteComment(id: number) {
    return this.http.delete<Comment[]>(this.baseUrl + 'admin/comment/' + id);
  }

  getHodnoceni() {
    return this.http.get<Hodnoceni[]>(this.baseUrl + 'admin/hodnoceni')
  }

  deleteHodnoceni(id: number) {
    return this.http.delete<Hodnoceni[]>(this.baseUrl + 'admin/hodnoceni/' + id);
  }

  getSoubory() {
    return this.http.get<Soubor[]>(this.baseUrl + 'admin/soubory')
  }

  deleteSoubor(id: number) {
    return this.http.delete<Soubor[]>(this.baseUrl + 'admin/soubor/' + id);
  }
}
