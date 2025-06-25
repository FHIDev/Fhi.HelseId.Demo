import { Component, OnInit } from '@angular/core';
import { AuthenticationService, User } from '../authentication.service';
import { catchError, map, Observable, of } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { jwtDecode } from "jwt-decode";

type UserSessionDto = {
  accessToken: string,
  idToken: string
}

type TokenInformation = {
  accessToken: string,
  accessTokenJwt: string,
  idToken: string,
  idTokenJwt:string
}


@Component({
    selector: 'app-user-session',
    templateUrl: './user-session.component.html',
    imports: []
})


export class UserSessionComponent implements OnInit  {
  public session$: Observable<User>;
  public isAuthenticated$: Observable<boolean>;
  public isAnonymous$: Observable<boolean>;
  public tokenInformation: TokenInformation | null = {
    accessToken: '',
    idToken: '',
    accessTokenJwt: '',
    idTokenJwt: ''
  };

  constructor(auth: AuthenticationService, private http: HttpClient) {
    this.session$ = auth.getSession();
    this.isAuthenticated$ = auth.getIsAuthenticated();
    this.isAnonymous$ = auth.getIsAnonymous();
  }

  ngOnInit() {
    this.http.get<UserSessionDto>('/api/v1/user-session').pipe(
      map(result => {
        const decodedAccessToken = jwtDecode(result.accessToken);
        const decodedIdToken = jwtDecode(result.idToken);
        return {
          accessToken: result.accessToken,
          accessTokenJwt: JSON.stringify(decodedAccessToken),
          idToken: result.idToken,
          idTokenJwt: JSON.stringify(decodedIdToken)
        };
      }),
      catchError(error => {
        console.error(error);
        return of(null);
      })
    ).subscribe(tokenInformation => {
      this.tokenInformation = tokenInformation;
    });
}
}
