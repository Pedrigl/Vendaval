import { Component, OnInit } from '@angular/core';
import { ProductService } from '../../product/product.service';
import { ApiResponse } from '../../shared/common/interfaces/apiResponse';
import { Product } from '../../product/product';
import { ProductType } from '../../product/product-type';
import { Router } from '@angular/router';
import { lastValueFrom } from 'rxjs';
import { LoadingService } from '../../shared/common/loading.service';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css']
})
export class ProductComponent implements OnInit {
  products!: ApiResponse<Product[]>;
  productType = ProductType;
  imagesLink: string = '';
  hasError = false;
  error!: string | null;

  constructor(private router: Router, private loadingService: LoadingService, private productService: ProductService) { }

  async ngOnInit() {
    try {
      this.loadingService.isLoading.next(true);
      this.products = await lastValueFrom(this.productService.getAllProducts());
      this.loadingService.isLoading.next(false);
    } catch (error: any) {
      this.hasError = true;
      this.error = error;
    }
  }

  editProduct(id: number) {
    this.router.navigate(['/admin/products/edit'], { queryParams: { id: id } });
  }

  async deleteProduct(id: number) {
    try {
      var req = await lastValueFrom(this.productService.deleteProduct(id));

      if (!req.success) {
        this.hasError = true;
        this.error = req.message;
      } else {
        this.hasError = false;
        this.error = '';
        this.products.data = this.products.data.filter(p => p.id !== id);
      }
    } catch (error: any) {
      this.hasError = true;
      this.error = error;
    }
  }
}
