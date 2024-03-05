import { KeyValue } from "@angular/common";
import { HttpClient, HttpErrorResponse, HttpHandler, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, catchError, throwError } from "rxjs";

@Injectable({
  providedIn: "root"
})
@Injectable()
export class AuthorizedHttpClient {
  constructor(private client: HttpClient) {
  }

  addHeaders(headers: KeyValue<string, string>[]): HttpHeaders {
    let httpHeaders = new HttpHeaders();

    headers.forEach(header => {
      httpHeaders = httpHeaders.append(header.key, header.value);
    });

    return httpHeaders;
  }
  
  getAuthToken(): string {
    
    let token = sessionStorage.getItem("token") ?? localStorage.getItem("token");

    if (token === null)
      throw new Error("You are not logged in");

    return token;
  }

  get<T>(url: string, headers?: KeyValue<string, string>[]): Observable<T> {
    let token = this.getAuthToken();

    if (!headers)
      headers = [];

    headers.push({ key: "Authorization", value: `Bearer ${token}` });
    return this.client.get<T>(url, { headers: this.addHeaders(headers) }).pipe(
      catchError((error: HttpErrorResponse) => {
        if(error.status == 401){
            localStorage.removeItem("token");

        return throwError(() => "Your session expired, you can only be logged in for one day, please login again");
        }

        if(error.status == 400){
            return throwError(() => error.error.message);
        }
        return throwError(() => error.message);
      }));
  }

  post<T>(url: string, body: any, headers?: KeyValue<string, string>[]): Observable<T> {
    let token = this.getAuthToken();

    if (!headers)
      headers = [];

    headers.push({ key: "Authorization", value: `Bearer ${token}` });
    return this.client.post<T>(url, body, { headers: this.addHeaders(headers) }).pipe(
      catchError((error: HttpErrorResponse) => {
        if(error.status == 401){
            localStorage.removeItem("token");

        return throwError(() => "Your session expired, you can only be logged in for one day, please login again");
        }

        if(error.status == 400){
            return throwError(() => error.error.message);
        }
        return throwError(() => error.message);
      }));
  }

  put<T>(url: string, body: any, headers?: KeyValue<string, string>[]): Observable<T> {
    let token = this.getAuthToken();

    if (!headers)
      headers = [];

    headers.push({ key: "Authorization", value: `Bearer ${token}` });
    return this.client.put<T>(url, body, { headers: this.addHeaders(headers) }).pipe(
      catchError((error: HttpErrorResponse) => {
        if(error.status == 401){
            localStorage.removeItem("token");

        return throwError(() => "Your session expired, you can only be logged in for one day, please login again");
        }

        if(error.status == 400){
            return throwError(() => error.error.message);
        }
        return throwError(() => error.message);
      }));
  }

  patch<T>(url: string, body: any, headers?: KeyValue<string, string>[]): Observable<T> {
    let token = this.getAuthToken();

    if (!headers)
      headers = [];

    headers.push({ key: "Authorization", value: `Bearer ${token}` });
    return this.client.patch<T>(url, body, { headers: this.addHeaders(headers) }).pipe(
      catchError((error: HttpErrorResponse) => {
        if(error.status == 401){
            localStorage.removeItem("token");

        return throwError(() => "Your session expired, you can only be logged in for one day, please login again");
        }

        if(error.status == 400){
            return throwError(() => error.error.message);
        }
        return throwError(() => error.message);
      }));
  }

  delete<T>(url: string, headers?: KeyValue<string, string>[]): Observable<T> {
    let token = this.getAuthToken();

    if (!headers)
      headers = [];

    headers.push({ key: "Authorization", value: `Bearer ${token}` });
    return this.client.delete<T>(url, { headers: this.addHeaders(headers) }).pipe(
      catchError((error: HttpErrorResponse) => {
        if(error.status == 401){
            localStorage.removeItem("token");

        return throwError(() => "Your session expired, you can only be logged in for one day, please login again");
        }

        if(error.status == 400){
            return throwError(() => error.error.message);
        }
        return throwError(() => error.message);
      }));
  }
  
}
