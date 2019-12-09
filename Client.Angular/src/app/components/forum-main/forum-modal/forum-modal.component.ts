import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap';
import { ForumService } from 'src/app/services/forum.service';
import { SubforumService } from 'src/app/services/subforum.service';

class ForumInput {
  title: string;
  description: string;
}

class SubforumInput {
  forumId: number;
  title: string;
  description: string;
  icon: string;
}

@Component({
  selector: 'app-forum-modal',
  templateUrl: './forum-modal.component.html',
  styleUrls: ['./forum-modal.component.css']
})
export class ForumModalComponent implements OnInit {
  @Output() refreshForum = new EventEmitter();
  title: string;
  description: string;
  icon: string;
  forSubforum: boolean;
  forumId: number;

  constructor(
    public bsModalRef: BsModalRef,
    private forumService: ForumService,
    private subforumService: SubforumService) { }

  ngOnInit() {
  }

  add() {
    if (!this.forSubforum) {
      const jsonBody = new ForumInput();
      jsonBody.title = this.title;
      jsonBody.description = this.description;

      this.forumService.addForum(jsonBody).subscribe(res => {
        this.refreshForum.emit(res);
      });
    } else {
      const jsonBody = new SubforumInput();
      jsonBody.forumId = this.forumId;
      jsonBody.title = this.title;
      jsonBody.description = this.description;
      jsonBody.icon = this.icon;

      this.subforumService.addSubforum(jsonBody).subscribe(res => {
        this.refreshForum.emit(res);
      });
    }

    this.bsModalRef.hide();
  }
}
