import { BsModalService, BsModalRef, ModalOptions } from 'ngx-bootstrap';
import { Component, OnInit } from '@angular/core';
import { NavModalComponent } from './nav-modal/nav-modal.component';
import { AuthService } from 'src/app/services/auth.service';
import { User } from 'src/app/models/user';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  bsModalRef: BsModalRef;
  loggedIn: boolean;
  user: User;

  constructor(private modalService: BsModalService, private auth: AuthService) { }

  ngOnInit() {
    this.auth.loggedIn.subscribe(res => {
      this.loggedIn = res;
    });
    this.auth.user.subscribe((user: User) => {
      this.user = user;
    });
    this.modalService.config.backdrop = 'static';
  }

  logout() {
    this.auth.logout();
  }

  openLoginModal() {
    const initialState = [];
    this.bsModalRef = this.modalService.show(NavModalComponent, { initialState });
  }
}
