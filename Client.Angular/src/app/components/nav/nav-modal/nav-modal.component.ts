import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { BsModalRef, ModalOptions } from 'ngx-bootstrap';
import { UserLoginDTO } from 'src/app/models/authDTO';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-nav-modal',
  templateUrl: './nav-modal.component.html',
  styleUrls: ['./nav-modal.component.css']
})
export class NavModalComponent implements OnInit {
  username: string;
  password: string;
  persisted = false;

  constructor(public bsModalRef: BsModalRef, private authService: AuthService) { }

  ngOnInit() {
  }

  login() {
    const jsonBody = new UserLoginDTO();
    jsonBody.username = this.username;
    jsonBody.password = this.password;

    this.authService.login(jsonBody, this.persisted).subscribe(() => { });
    this.bsModalRef.hide();
  }
}
