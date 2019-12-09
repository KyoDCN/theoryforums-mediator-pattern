import { BsModalRef } from 'ngx-bootstrap';
import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { ThreadService } from 'src/app/services/thread.service';
import { ThreadAddDTO, Thread } from 'src/app/models/thread';

@Component({
  selector: 'app-subforum-modal',
  templateUrl: './subforum-modal.component.html',
  styleUrls: ['./subforum-modal.component.css']
})
export class SubforumModalComponent implements OnInit {
  @Output() refreshThread = new EventEmitter();
  subforumId: number;
  title: string;
  content: string;

  constructor(private threadService: ThreadService, public bsModalRef: BsModalRef) { }

  ngOnInit() {
  }

  addThread() {
    const jsonBody = new ThreadAddDTO();
    jsonBody.subforumId = this.subforumId;
    jsonBody.title = this.title;
    jsonBody.content = this.content;

    this.threadService.addThread(jsonBody).subscribe((res: Array<Thread>) => {
      this.refreshThread.emit(res);
    });
    this.bsModalRef.hide();
  }
}
