import { environment } from './../../environments/environment';
import { PostAddDTO, PostUpdateDTO, Post } from './../models/post';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PostService {
  baseUrl: string = environment.baseUrl;

  constructor(private http: HttpClient) { }

  getToken() {
    return new HttpHeaders().set('Authorization', `Bearer ${sessionStorage.getItem('token')}`);
  }

  addPost(jsonBody: PostAddDTO): Observable<any> {
    return this.http.post(`${this.baseUrl}Post`, jsonBody, { headers: this.getToken() });
  }

  updatePost(postId: number, jsonBody: PostUpdateDTO) {
    return this.http.put(`${this.baseUrl}Post/${postId}`, jsonBody, { headers: this.getToken() });
  }

  deletePost(postId: number) {
    return this.http.delete(`${this.baseUrl}Post/${postId}`, { headers: this.getToken() });
  }

  getPost(postId: number): Observable<Post> {
    return this.http.get<Post>(`${this.baseUrl}Post/${postId}`);
  }

  getPosts(threadId: number): Observable<any> {
    return this.http.get(`${this.baseUrl}Post/threadId/${threadId}`);
  }
}
