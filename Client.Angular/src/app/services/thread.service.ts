import { environment } from './../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ThreadAddDTO, ThreadUpdateDTO } from '../models/thread';

@Injectable({
  providedIn: 'root'
})
export class ThreadService {
  baseUrl: string = environment.baseUrl;

  constructor(private http: HttpClient) { }

  addThread(jsonBody: ThreadAddDTO): Observable<any> {
    return this.http.post(`${this.baseUrl}thread`, jsonBody);
  }

  updateThread(threadId: number, jsonBody: ThreadUpdateDTO): Observable<any> {
    return this.http.put(`${this.baseUrl}thread/${threadId}`, jsonBody);
  }

  deleteThread(threadId: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}thread/${threadId}`);
  }

  getThread(threadId: number): Observable<any> {
    return this.http.get(`${this.baseUrl}thread/${threadId}`);
  }

  getThreads(subforumId: number): Observable<any> {
    return this.http.get(`${this.baseUrl}thread/subforumId/${subforumId}`);
  }

  incrementThreadViewCount(threadId: number): void {
    this.http.post(`${this.baseUrl}thread/${threadId}/view`, null).subscribe();
  }
}
