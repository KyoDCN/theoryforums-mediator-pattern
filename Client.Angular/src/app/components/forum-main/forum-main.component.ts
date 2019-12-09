import { SubforumBannerService } from 'src/app/services/subforum-banner.service';
import { ForumModalComponent } from './forum-modal/forum-modal.component';
import { Component, OnInit, Input } from '@angular/core';
import { Forum } from 'src/app/models/forum';
import { ForumService } from 'src/app/services/forum.service';
import { SubforumService } from 'src/app/services/subforum.service';
import { BsModalService, BsModalRef } from 'ngx-bootstrap';
import { SubforumUpdateDTO, SubforumAddDTO } from 'src/app/models/subforum';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-forum-main',
  templateUrl: './forum-main.component.html',
  styleUrls: ['./forum-main.component.css']
})
export class ForumMainComponent implements OnInit {
  forums: Array<Forum>;
  bsModalRef: BsModalRef;

  constructor(
    private forumService: ForumService,
    private subforumService: SubforumService,
    private modalService: BsModalService,
    private router: ActivatedRoute) {
  }

  ngOnInit() {
    this.router.data.subscribe(res => {
      this.forums = res.forum;
    });
  }

  getForums() {
    this.forumService.getForums().subscribe((res: Array<Forum>) => {
      this.forums = res;
    });
  }

  updateForum(forumId: number) {

  }

  deleteForum(forumId: number) {
    this.forumService.deleteForum(forumId).subscribe((res: Array<Forum>) => {
      this.forums = res;
    });
  }

  addSubforum(jsonBody: SubforumAddDTO) {
    this.subforumService.addSubforum(jsonBody).subscribe((res: Array<Forum>) => {
      this.forums = res;
    });
  }

  deleteSubforum(subforumId: number) {
    this.subforumService.deleteSubforum(subforumId).subscribe((res: Array<Forum>) => {
      this.forums = res;
    });
  }

  updateSubforum(subforumId: number, jsonBody: SubforumUpdateDTO) {
    this.subforumService.updateSubforum(subforumId, jsonBody).subscribe((res: Array<Forum>) => {
      this.forums = res;
    });
  }

  openAddForumModal() {
    const initialState = {
      forSubforum: false
    };
    this.bsModalRef = this.modalService.show(ForumModalComponent, { initialState });
    this.bsModalRef.content.refreshForum.subscribe((x: Array<Forum>) => this.forums = x);
  }

  openAddSubforumModal(forumId: number) {
    const initialState = {
      forSubforum: true,
      forumId
    };
    this.bsModalRef = this.modalService.show(ForumModalComponent, { initialState });
    this.bsModalRef.content.refreshForum.subscribe((x: Array<Forum>) => this.forums = x);
  }
}
