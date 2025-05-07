import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, Observable, retry, throwError } from 'rxjs';
import { Loan } from '../interfaces/loan';

@Injectable({
    providedIn: 'root'
})
export class LoanService {

    endPoint: string = "https://localhost:7130/api";

    constructor(private readonly httpClient: HttpClient) { }

    getLoans(): Observable<Loan[]> {
        return this.httpClient.get<Loan[]>(this.endPoint + "/loan")
            .pipe(
                retry(1),
                catchError(() => {
                    return throwError(() => new Error(this.httpError()));
                })
            );
    }

    getLoan(id: string): Observable<Loan> {
        return this.httpClient.get<Loan>(this.endPoint + "/loan/" + id)
            .pipe(
                retry(1),
                catchError(() => {
                    return throwError(() => new Error(this.httpError()));
                })
            );
    }

    addLoan(newLoan:any):Observable<Loan>{
        return this.httpClient.post<Loan>(this.endPoint + "/loan/", newLoan)
        .pipe(
          retry(1),
          catchError(() => {
            return throwError(() => new Error(this.httpError()));
          })
        );
      }

    updateLoan(data: Loan): Observable<Loan> {
        return this.httpClient.patch<Loan>(this.endPoint + "/loan/" + data.id, data)
            .pipe(
                retry(1),
                catchError(() => {
                    return throwError(() => new Error(this.httpError()));
                })
            );
    }

    deleteUser(id:string):Observable<Loan>{
        return this.httpClient.delete<Loan>(this.endPoint + "/loan/" + id )
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