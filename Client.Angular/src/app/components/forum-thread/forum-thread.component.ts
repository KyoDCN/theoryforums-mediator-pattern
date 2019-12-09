import { Delta } from 'quill/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { Component, OnInit } from '@angular/core';
import { Post, PostAddDTO } from 'src/app/models/post';
import { Thread } from 'src/app/models/thread';
import { ActivatedRoute } from '@angular/router';
import { PostService } from 'src/app/services/post.service';
import Quill from 'quill';
import { SubforumBannerService } from 'src/app/services/subforum-banner.service';

@Component({
  selector: 'app-forum-thread',
  templateUrl: './forum-thread.component.html',
  styleUrls: ['./forum-thread.component.css']
})
export class ForumThreadComponent implements OnInit {
  posts: Array<Post> = new Array<Post>();
  thread: Thread = new Thread();

  editorOpen: boolean;
  threadContent: Quill;

  constructor(private router: ActivatedRoute, private post: PostService, private subforumBannerService: SubforumBannerService) { }

  ngOnInit() {
    this.editorOpen = false;

    this.router.data.subscribe(data => {
      data.thread.thread.subscribe((x: Thread) => {
        this.thread = x;
        this.subforumBannerService.subforum.next(this.thread.subforum);
        this.initQuill();
      });
      data.thread.posts.subscribe((x: Array<Post>) => {
        this.posts = x;
      });
    });
  }

  private initQuill() {
    this.threadContent = new Quill('#thread-content');
    this.threadContent.disable();

    if ((this.thread.content as string).includes('[', 0)) {
      this.threadContent.setContents(JSON.parse(this.thread.content));
    } else {
      this.threadContent.setText(this.thread.content);
    }
  }

  openEditor() {
    this.editorOpen = !this.editorOpen;
  }

  submitPost(content: Delta) {
    const jsonBody = new PostAddDTO();
    jsonBody.threadId = this.thread.id;
    jsonBody.content = content;

    this.post.addPost(jsonBody).subscribe((x: Array<Post>) => {
      this.posts = x;
    });
  }
}
