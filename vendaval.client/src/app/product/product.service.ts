import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthorizedHttpClient } from '../shared/common/authorized-httpclient';
import { environment } from '../../environments/environment';
import { ApiResponse } from '../shared/common/interfaces/apiResponse';
import { Product } from './product';
import { KeyValue, KeyValuePipe } from '@angular/common';
import { CreatePreAuthenticatedRequest } from '../shared/common/interfaces/createPreAuthenticatedRequest';
import { ImageNames } from '../shared/common/interfaces/imageNames';

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

  public uploadImage(image: File) {
    const formData = new FormData();
    formData.append('image', image);
    
    return this.authClient.post<ApiResponse<object>>(environment.apiUrl + `Product/uploadProductImage`, formData);
  }

  public CreateAuthRequestToProductImagesLink() {
    return this.httpClient.get<ApiResponse<CreatePreAuthenticatedRequest>>(environment.apiUrl + 'Product/getLinkToProductImages');
  }

  public getProductImagesNames(preAuthenticatedLink: string) {
    return this.httpClient.get<ImageNames>(preAuthenticatedLink);
  }

  public deleProductImage(imageName: string) {
    return this.authClient.delete<ApiResponse<object>>(environment.apiUrl + `Product/deleteProductImage?imageName=${imageName}`);
  }
  public updateProduct(product: Product) {
    return this.authClient.put<ApiResponse<Product>>(environment.apiUrl + 'Product/updateProduct', product);
  }

  public deleteProduct(id: number) {
    return this.authClient.delete<ApiResponse<Product>>(environment.apiUrl + 'Product/deleteProduct?id=' + id);
  }
}
