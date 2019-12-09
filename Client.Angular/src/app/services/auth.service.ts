import { Router } from '@angular/router';
import { environment } from './../../environments/environment';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserRegisterDTO, UserLoginDTO } from '../models/authDTO';
import { User } from '../models/user';
import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl: string = environment.baseUrl + 'auth/';
  user = new BehaviorSubject<User>(new User());
  loggedIn = new BehaviorSubject<boolean>(!!localStorage.getItem('token'));
  jwtHelper = new JwtHelperService();

  constructor(private http: HttpClient) {
    if (this.checkTokenExp()) {
      localStorage.removeItem('token');
      sessionStorage.removeItem('token');
      localStorage.removeItem('avatar');
      sessionStorage.removeItem('avatar');
      this.loggedIn.next(false);
      this.user.next(new User());
    } else {
      this.user.next(this.decodeTokenUser());
      this.loggedIn.next(true);
    }
  }

  register(jsonBody: UserRegisterDTO) {
    return this.http.post(`${this.baseUrl}register`, jsonBody).pipe(
      map((res: any) => {
      })
    );
  }

  login(jsonBody: UserLoginDTO, isPersisted: boolean = true) {
    return this.http.post(`${this.baseUrl}login`, jsonBody).pipe(
      map((res: any) => {
        this.setUser(res, isPersisted);
      })
    );
  }

  // login(jsonBody: UserLoginDTO, isPersisted: boolean = true) {
  //   this.http.post(`${this.baseUrl}login`, jsonBody).subscribe((res: any) => {
  //     this.setUser(res, isPersisted);
  //   });
  // }

  private setUser(res, isPersisted: boolean) {
    if (isPersisted) {
      localStorage.setItem('token', res.token);
      localStorage.setItem('avatar', res.avatarUrl);
    } else {
      sessionStorage.setItem('token', res.token);
      sessionStorage.setItem('avatar', res.avatarUrl);
    }

    const user = this.decodeTokenUser();

    this.user.next(user);
    this.loggedIn.next(true);
  }

  decodeTokenUser(): User {
    let decodedToken: any;
    const user = new User();

    if (!!localStorage.getItem('token')) {
      decodedToken = this.jwtHelper.decodeToken(localStorage.getItem('token'));
      user.avatarUrl = localStorage.getItem('avatar');
    } else if (!!sessionStorage.getItem('token')) {
      decodedToken = this.jwtHelper.decodeToken(sessionStorage.getItem('token'));
      user.avatarUrl = sessionStorage.getItem('avatar');
    }

    user.id = decodedToken.nameid;
    user.displayName = decodedToken.unique_name;
    user.roles = decodedToken.role;

    return user;
  }

  updateUserEvent(roles: string[] = this.user.value.roles, avatar: string = this.user.value.avatarUrl) {
    const user = this.user.value;
    user.roles = roles;
    user.avatarUrl = avatar;
    this.user.next(user);
  }

  logout() {
    localStorage.removeItem('token');
    sessionStorage.removeItem('token');
    localStorage.removeItem('avatar');
    sessionStorage.removeItem('avatar');
    this.loggedIn.next(false);
  }

  checkTokenExp() {
    const localToken = localStorage.getItem('token');
    const sessionToken = sessionStorage.getItem('token');
    if (localToken !== null) {
      return this.jwtHelper.isTokenExpired(localToken);
    } else if (sessionToken !== null) {
      return this.jwtHelper.isTokenExpired(sessionToken);
    } else {
      return true;
    }
  }
}
