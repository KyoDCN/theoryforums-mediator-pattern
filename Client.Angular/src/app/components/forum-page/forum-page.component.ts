import { Component, OnInit, DoCheck } from '@angular/core';
import { Location } from '@angular/common';

@Component({
  selector: 'app-forum-page',
  templateUrl: './forum-page.component.html',
  styleUrls: ['./forum-page.component.css']
})
export class ForumPageComponent implements OnInit, DoCheck {
  show: boolean;
  registerBackground: boolean;

  constructor(private location: Location) {

  }

  ngOnInit() {
  }

  ngDoCheck() {
    this.show = this.location.path().includes('subforum') || this.location.path().includes('thread');
    this.registerBackground = this.location.path().includes('register');
  }
}
