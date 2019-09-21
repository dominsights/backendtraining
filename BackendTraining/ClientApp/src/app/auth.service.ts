import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { shareReplay, tap, catchError } from 'rxjs/operators';
import * as moment from "moment";

import { User } from './User';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
  }

  login(user: User) {
    if (user.userName !== '' && user.password !== '') {
      //Verify if response is valid ???
      return this.http.post<User>(this.baseUrl + 'api/Auth/Login', user)
        .pipe(
          tap(this.setSession),
          shareReplay()
        );
    }
  }

  logout() {
    localStorage.removeItem("token");
    localStorage.removeItem("expires_at");
  }

  public isLoggedIn() {
    return moment().isBefore(this.getExpiration());
  }

  isLoggedOut() {
    return !this.isLoggedIn();
  }

  getExpiration() {
    const expiration = localStorage.getItem("expires_at");
    const expiresAt = JSON.parse(expiration);
    return moment(expiresAt);
  }

  private setSession(authResult) {
    const expiresAt = moment().add(authResult.expiresIn, 'second');

    localStorage.setItem('token', authResult.token);
    localStorage.setItem('expires_at', JSON.stringify(expiresAt.valueOf()));
  }
}
