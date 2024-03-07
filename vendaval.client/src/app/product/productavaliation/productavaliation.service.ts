import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthorizedHttpClient } from '../../shared/common/authorized-httpclient';
import { ApiResponse } from '../../shared/common/interfaces/apiResponse';
import { ProductAvaliation } from './productavaliation';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ProductavaliationService {

  constructor(private httpClient: HttpClient, private authClient: AuthorizedHttpClient) { }

  public getAvaliations(productId: number) {
    return this.httpClient.get<ApiResponse<ProductAvaliation[]>>(environment.apiUrl + `ProductAvaliation/getByProductId?productId=${productId}`);
  }

  public addAvaliation(avaliation: ProductAvaliation) {
    return this.authClient.post<ApiResponse<ProductAvaliation>>(environment.apiUrl + 'ProductAvaliation/add', avaliation);
  }

  public updateAvaliation(avaliation: ProductAvaliation) {
    return this.authClient.put<ApiResponse<ProductAvaliation>>(environment.apiUrl + 'ProductAvaliation/update', avaliation);
  }

  public deleteAvaliation(productId: number,avaliationId: number) {
    return this.authClient.delete<ApiResponse<ProductAvaliation>>(environment.apiUrl + `ProductAvaliation/delete?productId=${productId}&avaliationId=${avaliationId}`);
  }
}
