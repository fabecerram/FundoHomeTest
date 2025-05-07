import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, Observable, retry, throwError } from 'rxjs';
import { Login } from '../interfaces/Login';

@Injectable({
    providedIn: 'root'
})
export class LoginService {

    endPoint: string = "https://localhost:7130/api";

    constructor(private readonly httpClient: HttpClient) { }

    login(login: Login): Observable<Login> {
        return this.httpClient.post<Login>(this.endPoint + "/login", login)
            .pipe(
                retry(1),
                catchError(() => {
                    return throwError(() => new Error(this.httpError()));
                })
            );
    }

    refresh(login: Login): Observable<Login> {
        return this.httpClient.post<Login>(this.endPoint + "/token/refresh", login)
            .pipe(
                retry(1),
                catchError(() => {
                    return throwError(() => new Error(this.httpError()));
                })
            );
    }

    logout(login: Login): Observable<Login> {
        return this.httpClient.post<Login>(this.endPoint + "token/revoke", login)
            .pipe(
                retry(1),
                catchError(() => {
                    return throwError(() => new Error(this.httpError()));
                })
            );
    }

    httpError(): string {
        return "An error has occurred, please contact your administrator."
    }
}