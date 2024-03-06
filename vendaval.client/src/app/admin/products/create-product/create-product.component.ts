import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ProductService } from '../../../product/product.service';
import { Product } from '../../../product/product';
import { ProductType } from '../../../product/product-type';
import { lastValueFrom } from 'rxjs';

@Component({
  selector: 'app-create-product',
  templateUrl: './create-product.component.html',
  styleUrl: './create-product.component.css'
})
export class CreateProductComponent {
  product: Product = {
    id: 0,
    name: '',
    price: 0.00,
    description: '',
    avaliation: 0.00,
    stock: 0,
    image: '',
    category: ProductType.Clothing
  }
  productType = ProductType;
  productTypesNames = Object.keys(ProductType).filter(key => isNaN(Number(key)));
  productTypesValues = Object.values(ProductType).filter(value => !isNaN(Number(value)));
  hasError = false;
  error!: string | null;

  constructor(private router: Router, private productService: ProductService) {
  }

  async createProduct() {
    try {
      this.product.category = Number(this.product.category);
      console.log(this.product);
      var res = await lastValueFrom(this.productService.createProduct(this.product));

      if (res.success) {
        this.hasError = false;
        this.error = '';

        this.router.navigate(['/admin/products']);
      }

      else {
        this.hasError = true;
        this.error = res.message;
      }
    }
    catch (error: any) {
      this.hasError = true;
      console.log(error);
      this.error = error;
    }
  }
}
