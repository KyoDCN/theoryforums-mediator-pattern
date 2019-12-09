import { Component, OnInit } from '@angular/core';
import { UserRegisterDTO } from 'src/app/models/authDTO';
import { AuthService } from 'src/app/services/auth.service';
import { Router } from '@angular/router';
import { FormGroup, Validators, AbstractControl, FormBuilder } from '@angular/forms';
import { Observable } from 'rxjs';
import { SubforumBannerService } from 'src/app/services/subforum-banner.service';
import { Location } from '@angular/common';

class Error {
  duplicateUsername: string;
  duplicateEmail: string;
}

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  public registerForm: FormGroup;
  public rf: {
    [key: string]: AbstractControl
  };
  public errors: Error = new Error();

  constructor(
    private auth: AuthService,
    private router: Router,
    private formBuilder: FormBuilder) {
  }

  ngOnInit() {
    this.registerForm = this.formBuilder.group({
      // For reference if ever doing custom validators
      // tslint:disable-next-line: max-line-length
      // displayName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(26)], this.displayNameNoSpecialCharValidator],
      username: ['', [Validators.required, Validators.minLength(6), Validators.pattern(/^[\w]*[^_\W\b]$/)]],
      displayName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(26), Validators.pattern(/^[\w ]*[^_\W\b]$/)]],
      email: ['', [
        Validators.required,
        // tslint:disable-next-line: max-line-length
        Validators.pattern(/^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?/),
        Validators.maxLength(40)]
      ],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required]]
    }, {
      validator: [this.passwordMatchValidator, this.displayNameMatchValidator]
    });
    this.rf = this.registerForm.controls;
  }

  register() {
    const jsonBody = new UserRegisterDTO();
    jsonBody.username = this.registerForm.value.username;
    jsonBody.displayName = this.registerForm.value.displayName;
    jsonBody.email = this.registerForm.value.email;
    jsonBody.password = this.registerForm.value.password;

    this.auth.register(jsonBody).subscribe(() => {
      this.router.navigate(['/forum']);
    }, err => {
      err.error.forEach(element => {
        if (element.code === 'DuplicateUserName') {
          this.errors.duplicateUsername = element.description;
        }
        if (element.code === 'DuplicateEmail') {
          this.errors.duplicateEmail = element.description;
        }
      });
    });
  }

  clearErrors(input: string) {
    if (this.errors.duplicateUsername || this.errors.duplicateEmail) {
      switch (input) {
        case 'username':
          this.errors.duplicateUsername = null;
          break;
        case 'email':
          this.errors.duplicateEmail = null;
          break;
      }
    }
  }

  passwordMatchValidator(x: FormGroup) {
    return x.get('password').value === x.get('confirmPassword').value ? null : { mismatch: true };
  }

  displayNameMatchValidator(x: FormGroup) {
    return (x.get('username').value !== x.get('displayName').value) || x.get('displayName').value === '' ? null : { matchUsername: true };
  }

  usernameNoSpecialCharValidator(x: AbstractControl): Observable<any> {
    return new Observable((observer) => {
      if (/^[\w]+[^_\s\W]$/g.test(x.value) === false) {
        observer.next({ invalidChar: true });
      } else {
        observer.next(null);
      }
      observer.complete();
      return { unsubscribe() { } };
    });
  }

  displayNameNoSpecialCharValidator(x: AbstractControl): Observable<any> {
    return new Observable((observer) => {
      if (/^[\w ]+[^_\s\W]$/g.test(x.value) === false) {
        observer.next({ invalidChar: true });
      } else {
        observer.next(null);
      }
      observer.complete();
      return { unsubscribe() { } };
    });
  }
}
