import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthorizedHttpClient } from '../shared/common/authorized-httpclient';
import { environment } from '../../environments/environment';
import { ApiResponse } from '../shared/common/interfaces/apiResponse';
import { Product } from './product';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  constructor(private httpClient: HttpClient, private authClient: AuthorizedHttpClient) { }

  public getAllProducts() {
    return this.httpClient.get<ApiResponse<Product[]>>(environment.apiUrl +'Product/getProducts');
  }

  public getProductById(id: number) {
    return this.httpClient.get<ApiResponse<Product>>(environment.apiUrl + 'Product/getProductById?id=' + id);
  }

  public createProduct(product: Product) {
    return this.authClient.post<ApiResponse<Product>>(environment.apiUrl + 'Product/registerProduct', product);
  }

  public updateProduct(product: Product) {
    return this.authClient.put<ApiResponse<Product>>(environment.apiUrl + 'Product/updateProduct', product);
  }

  public deleteProduct(id: number) {
    return this.authClient.delete<ApiResponse<Product>>(environment.apiUrl + 'Product/deleteProduct?id=' + id);
  }
}
