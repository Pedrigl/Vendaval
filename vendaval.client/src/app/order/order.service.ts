import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthorizedHttpClient } from '../shared/common/authorized-httpclient';
import { environment } from '../../environments/environment';
import { ApiResponse } from '../shared/common/interfaces/apiResponse';
import { Order } from './order';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  constructor(private authClient: AuthorizedHttpClient) { }

  public getOrdersByUser(userId: number) {
      return this.authClient.get<ApiResponse<Order[]>>(environment.apiUrl + "Order/getByUserId?userId=" + userId);
  }

  public createOrder(order: Order) {
    return this.authClient.post<ApiResponse<Order>>(environment.apiUrl + 'Order/createOrder', order);
  }

  public updateOrder(order: Order) {
    return this.authClient.put<ApiResponse<Order>>(environment.apiUrl + 'Order/updateOrder', order);
  }

  public deleteOrder(id: number) {
    return this.authClient.delete<ApiResponse<Order>>(environment.apiUrl + 'Order/deleteOrder?orderId=' + id);
  }

  public getAllOrders() {
    return this.authClient.get<ApiResponse<Order[]>>(environment.apiUrl + 'Order/getAll');
  }
}
