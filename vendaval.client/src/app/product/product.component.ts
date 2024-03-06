import { Component } from '@angular/core';
import { ProductService } from './product.service';
import { ApiResponse } from '../shared/common/interfaces/apiResponse';
import { Product } from './product';
import { ProductType } from './product-type';

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


}
