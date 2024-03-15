import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { LoadingService } from '../shared/common/loading.service';
import { ProductService } from '../product/product.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  constructor(private router: Router, private productService: ProductService) {}
}
