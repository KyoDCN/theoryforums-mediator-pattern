import { Subforum } from './../models/subforum';
import { Injectable, EventEmitter, Output } from '@angular/core';
import { Subject, Observable, BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SubforumBannerService {
  subforum = new BehaviorSubject<Subforum>(new Subforum());

  constructor() { }
}
