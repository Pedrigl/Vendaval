import { Component } from '@angular/core';
import { ProductService } from '../../product/product.service';
import { ApiResponse } from '../../shared/common/interfaces/apiResponse';
import { Product } from '../../product/product';
import { ProductType } from '../../product/product-type';
import { Router } from '@angular/router';
import { lastValueFrom } from 'rxjs';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrl: './product.component.css'
})
export class ProductComponent {
  products!: ApiResponse<Product[]>;
  productType = ProductType;
  hasError = false;
  error!: string | null;

  constructor(private router: Router, private productService: ProductService) {
    productService.getAllProducts().subscribe(response => {
      this.products = response;
    });
  }

  editProduct(id: number) {
    this.router.navigate(['/admin/products/edit'], { queryParams: { id: id } });
  }

  async deleteProduct(id: number) {
    try {
      var req = await lastValueFrom(this.productService.deleteProduct(id));

      if(!req.success) {
        this.hasError = true;
        this.error = req.message;
      }
    }
    catch (error: any) {
      this.hasError = true;
      this.error = error;
    }
  }
}
