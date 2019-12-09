import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ForumService {
  baseUrl = 'http://localhost:5000/api/forum';
  constructor(private http: HttpClient) { }

  getForums(): Observable<any> {
    return this.http.get(`${this.baseUrl}`);
  }

  addForum(jsonBody): Observable<any> {
    return this.http.post(`${this.baseUrl}`, jsonBody);
  }

  deleteForum(forumId: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${forumId}`);
  }

  updateForum(forumId: number, forumUpdate): Observable<any> {
    return this.http.put(`${this.baseUrl}/${forumId}`, forumUpdate);
  }
}
