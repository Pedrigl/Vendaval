import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ProductService } from '../../../product/product.service';
import { Product } from '../../../product/product';
import { lastValueFrom } from 'rxjs';

@Component({
  selector: 'app-edit-product',
  templateUrl: './edit-product.component.html',
  styleUrl: './edit-product.component.css'
})
export class EditProductComponent {
  product!: Product;
  hasError = false;
  error = '';
  constructor(private router: Router, private productService: ProductService) { }

  async save() {
    this.hasError = false;
    this.error = '';

    try {
      this.product.category = Number(this.product.category);
      var req = await lastValueFrom(this.productService.updateProduct(this.product));

      this.product = req.data;

      if(req.success){
        this.router.navigate(['/admin/products']);
      })

    }

    catch (error: any) {
      this.hasError = true;
      this.error = error;
    }
      
    }
}
