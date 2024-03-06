import { KeyValue } from "@angular/common";
import { HttpClient, HttpErrorResponse, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, throwError } from "rxjs";
import { catchError } from "rxjs/operators";

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

  handleError(error: HttpErrorResponse): Observable<never> {
    if (error.status == 400) {
      return throwError(() => error.error);
    }

    else if (error.status == 401) {
      return throwError(() => "You have to be logged in to acces this");
    }

    else if (error.status == 403) {
      return throwError(() => "You are not allowed to access this");
    }

    else if (error.status == 404) {
      return throwError(() => "The resource you are looking for does not exist");
    }

    else if (error.status == 500) {
      return throwError(() => "An error occurred on the server");
    }

    else if (error.status == 0) {
      return throwError(() => "Could not connect to the server");
    }

    return throwError(()=>error);
  }

  get<T>(url: string, headers?: KeyValue<string, string>[]): Observable<T> {
    let token = this.getAuthToken();

    if (!headers)
      headers = [];

    headers.push({ key: "Authorization", value: `Bearer ${token}` });
    return this.client.get<T>(url, { headers: this.addHeaders(headers) }).pipe(
      catchError(this.handleError));
  }

  post<T>(url: string, body: any, headers?: KeyValue<string, string>[]): Observable<T> {
    let token = this.getAuthToken();

    if (!headers)
      headers = [];

    headers.push({ key: "Authorization", value: `Bearer ${token}` });
    return this.client.post<T>(url, body, { headers: this.addHeaders(headers) }).pipe(
      catchError(this.handleError));
  }

  put<T>(url: string, body: any, headers?: KeyValue<string, string>[]): Observable<T> {
    let token = this.getAuthToken();

    if (!headers)
      headers = [];

    headers.push({ key: "Authorization", value: `Bearer ${token}` });
    return this.client.put<T>(url, body, { headers: this.addHeaders(headers) }).pipe(
      catchError(this.handleError));
  }

  patch<T>(url: string, body: any, headers?: KeyValue<string, string>[]): Observable<T> {
    let token = this.getAuthToken();

    if (!headers)
      headers = [];

    headers.push({ key: "Authorization", value: `Bearer ${token}` });
    return this.client.patch<T>(url, body, { headers: this.addHeaders(headers) }).pipe(
      catchError(this.handleError));
  }

  delete<T>(url: string, headers?: KeyValue<string, string>[]): Observable<T> {
    let token = this.getAuthToken();

    if (!headers)
      headers = [];

    headers.push({ key: "Authorization", value: `Bearer ${token}` });
    return this.client.delete<T>(url, { headers: this.addHeaders(headers) }).pipe(
      catchError(this.handleError));
  }
  
}
