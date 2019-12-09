import { BsModalRef } from 'ngx-bootstrap';
import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { PostAddDTO } from 'src/app/models/post';
import { PostService } from 'src/app/services/post.service';

@Component({
  selector: 'app-thread-modal',
  templateUrl: './thread-modal.component.html',
  styleUrls: ['./thread-modal.component.css']
})
export class ThreadModalComponent implements OnInit {
  @Output() refreshPost = new EventEmitter();
  threadId: number;
  content: string;

  constructor(public bsModalRef: BsModalRef, private postService: PostService) { }

  ngOnInit() {
  }

  addPost() {
    const jsonBody = new PostAddDTO();
    jsonBody.threadId = this.threadId;
    jsonBody.content = this.content;

    this.postService.addPost(jsonBody).subscribe(x => {
      this.refreshPost.emit(x);
    });
    this.bsModalRef.hide();
  }
}
