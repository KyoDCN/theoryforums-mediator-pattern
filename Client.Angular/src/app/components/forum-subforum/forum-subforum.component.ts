import { BsModalService, BsModalRef } from 'ngx-bootstrap';
import { Thread, ThreadAddDTO } from './../../models/thread';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subforum } from 'src/app/models/subforum';
import { ThreadService } from 'src/app/services/thread.service';
import { SubforumModalComponent } from './subforum-modal/subforum-modal.component';
import { SubforumBannerService } from 'src/app/services/subforum-banner.service';

@Component({
  selector: 'app-forum-subforum',
  templateUrl: './forum-subforum.component.html',
  styleUrls: ['./forum-subforum.component.css']
})
export class ForumSubforumComponent implements OnInit {
  subforum: Subforum = new Subforum();
  threads: Array<Thread> = new Array<Thread>();
  bsModalRef: BsModalRef;
  subforumId: number;

  constructor(
    private router: ActivatedRoute,
    private threadService: ThreadService,
    private modalService: BsModalService,
    private subforumBannerService: SubforumBannerService) { }

  ngOnInit() {
    this.router.data.subscribe(x => {
      x.subforum.threadlist.subscribe(res => {
        this.threads = res;
      });
      x.subforum.subforum.subscribe(res => {
        this.subforum = res;
        this.subforumBannerService.subforum.next(res);
      });
      this.subforumId = x.subforum.subforumId;
    });
  }

  addThread(jsonBody: ThreadAddDTO) {
    this.threadService.addThread(jsonBody).subscribe((res: Array<Thread>) => {
      this.threads = res;
    });
  }

  openAddNewThreadModal() {
    const initialState = {
      subforumId: this.subforumId
    };
    this.bsModalRef = this.modalService.show(SubforumModalComponent, { initialState });
    this.bsModalRef.content.refreshThread.subscribe((res: Array<Thread>) => {
      this.threads = res;
    });
  }
}
