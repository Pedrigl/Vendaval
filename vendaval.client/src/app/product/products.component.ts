import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { LoadingService } from '../shared/common/loading.service';
import { ProductService } from './product.service';
import { Product } from './product';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrl: './products.component.css'
})
export class ProductsComponent {
  products!: Product[];
  constructor(private router: Router, private loadingService: LoadingService, private productService: ProductService) {
    this.loadingService.isLoading.next(true);
    productService.getAllProducts().subscribe(response => {
      this.products = response.data;
      this.loadingService.isLoading.next(false);
    });
  }


}
