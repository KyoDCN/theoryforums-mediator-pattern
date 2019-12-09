import { Delta } from 'quill/core';
import { Component, OnInit, Input, AfterViewInit } from '@angular/core';
import { Post } from 'src/app/models/post';
import Quill from 'quill';
import { PostService } from 'src/app/services/post.service';

@Component({
  selector: 'app-forum-post',
  templateUrl: './forum-post.component.html',
  styleUrls: ['./forum-post.component.css']
})
export class ForumPostComponent implements OnInit, AfterViewInit {
  @Input() post: Post;
  quill: Quill;
  content: Delta;
  isEdit: boolean;

  constructor(private postService: PostService) { }

  ngOnInit() {
    this.isEdit = false;
  }

  ngAfterViewInit() {
    this.quill = new Quill(`#post-view-${this.post.id}`);
    this.quill.disable();

    if ((this.post.content as string).includes('[', 0)) {
      this.quill.setContents(JSON.parse(this.post.content));
    } else {
      this.quill.setText(this.post.content);
    }
  }

  editPost() {
    this.quill.enable();
    this.isEdit = true;
  }

  savePost() {
    this.post.content = this.quill.getContents();
    this.postService.updatePost(this.post.id, this.post).subscribe((res: Post) => {
      this.post = res;
      this.isEdit = false;
      this.quill.disable();
    }, err => {
      console.log(err);
    });
  }
}
