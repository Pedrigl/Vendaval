import { Component } from '@angular/core';
import { ProductService } from '../../product/product.service';
import { ApiResponse } from '../../shared/common/interfaces/apiResponse';
import { Product } from '../../product/product';
import { ProductType } from '../../product/product-type';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrl: './product.component.css'
})
export class ProductComponent {
  products!: ApiResponse<Product[]>;
  productType = ProductType;
  hasError = false;
  error = '';
  constructor(productService: ProductService) { }

  editProduct(id: number) {
    console.log(id);
  }

  async deleteProduct(id: number) {
    console.log(id);
  }

  async addProduct() {
    console.log('add');
  }

}
