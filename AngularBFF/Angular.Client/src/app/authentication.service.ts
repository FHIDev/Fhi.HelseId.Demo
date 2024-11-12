/*
This service is not in use in this application. But it can be convinient to use an authentication service to control login, logout and user session
*/


import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, filter, map, Observable, of, shareReplay } from 'rxjs';

const ANONYMOUS: User = null;
const CACHE_SIZE = 1;

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  private session$: Observable<User> | null = null
  constructor(private http: HttpClient) {
  }

  public login(){
    this.http.get('/Account/Login').pipe(
      catchError(err => {
        console.log(err);
        return of(ANONYMOUS);
      })
    )
  }

  public logout(){
    this.http.get('/Account/Logout').pipe(
      catchError(err => {
        console.log(err);
        return of(ANONYMOUS);
      })
    )
  }

  public getSession(ignoreCache: boolean = false) {
    if (!this.session$ || ignoreCache) {
      this.session$ = this.http.get<User>('user').pipe(
        catchError(err => {
          return of(ANONYMOUS);
        }),
        shareReplay(CACHE_SIZE)
      );
    }
    return this.session$;
  }

  public getIsAuthenticated(ignoreCache: boolean = false) {
    return this.getSession(ignoreCache).pipe(
      map(UserIsAuthenticated)
    );
  }

  public getIsAnonymous(ignoreCache: boolean = false) {
    return this.getSession(ignoreCache).pipe(
      map(UserIsAnonymous)
    );
  }

  public getUsername(ignoreCache: boolean = false) {
    return this.getSession(ignoreCache).pipe(
      filter(UserIsAuthenticated),
      map(s => s.name)
    );
  }

  
}

export type User = {
  id: string,
  name: string,
  hprNummer: string,
  pidPseudonym: string,
  pid: string,
  securityLevel: string,
  assuranceLevel: string,
  network: string
} | null;

function UserIsAuthenticated(s: User) {
  return s !== null;
}

function UserIsAnonymous(s: User): s is null {
  return s === null;
}
