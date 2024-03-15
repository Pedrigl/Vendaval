import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LoadingService } from '../shared/common/loading.service';
import { OrderService } from './order.service';
import { Order } from './order';
import { AuthService } from '../shared/common/auth.service';
import { lastValueFrom } from 'rxjs';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrl: './order.component.css'
})
export class OrderComponent implements OnInit{
  orders: Order[];
  constructor(private router: Router, private orderService: OrderService, private userService: AuthService) {
      this.orders = [];
  }

  async ngOnInit() {

    var user = await lastValueFrom(this.userService.getUser);

    if (!user) {
      this.router.navigate(['/login']);
      return
    }

    var orders = await lastValueFrom(this.orderService.getOrdersByUser(user.id));

    this.orders = orders.data;
  }
}
