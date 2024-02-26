import { KeyValue } from "@angular/common";
import { HttpClient, HttpHandler, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";

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
    let token = localStorage.getItem("token");
    if (!token)
      throw new Error("Token not found");

    return token
  }

  get(url: string, headers?: KeyValue<string, string>[]) {
    let token = this.getAuthToken();

    if (!headers)
      headers = [];

    headers.push({ key: "Authorization", value: `Bearer ${token}` });
    return this.client.get(url, { headers: this.addHeaders(headers) });
  }

  post(url: string, body: any, headers?: KeyValue<string, string>[]) {
    let token = this.getAuthToken();

    if (!headers)
      headers = [];

    headers.push({ key: "Authorization", value: `Bearer ${token}` });
    return this.client.post(url, body, { headers: this.addHeaders(headers) });
  }

  put(url: string, body: any, headers?: KeyValue<string, string>[]) {
    let token = this.getAuthToken();

    if (!headers)
      headers = [];

    headers.push({ key: "Authorization", value: `Bearer ${token}` });
    return this.client.put(url, body, { headers: this.addHeaders(headers) });
  }

  patch(url: string, body: any, headers?: KeyValue<string, string>[]) {
    let token = this.getAuthToken();

    if (!headers)
      headers = [];

    headers.push({ key: "Authorization", value: `Bearer ${token}` });
    return this.client.patch(url, body, { headers: this.addHeaders(headers) });
  }

  delete(url: string, headers?: KeyValue<string, string>[]) {
    let token = this.getAuthToken();

    if (!headers)
      headers = [];

    headers.push({ key: "Authorization", value: `Bearer ${token}` });
    return this.client.delete(url, { headers: this.addHeaders(headers) });
  }
  
}
