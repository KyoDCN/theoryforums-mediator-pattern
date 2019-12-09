import { SubforumAddDTO } from './../models/subforum';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { SubforumUpdateDTO } from '../models/subforum';

@Injectable({
  providedIn: 'root'
})
export class SubforumService {
  baseUrl = 'http://localhost:5000/api/subforum';

  constructor(private http: HttpClient) { }

  addSubforum(jsonBody: SubforumAddDTO): Observable<any> {
    return this.http.post(`${this.baseUrl}`, jsonBody);
  }

  deleteSubforum(subforumId: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${subforumId}`);
  }

  updateSubforum(subforumId: number, jsonBody: SubforumUpdateDTO): Observable<any> {
    return this.http.put(`${this.baseUrl}/${subforumId}`, jsonBody);
  }

  getSubforum(subforumId: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/${subforumId}`);
  }

  getSubforums(): Observable<any> {
    return this.http.get(`${this.baseUrl}`);
  }
}
