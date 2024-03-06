import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductService } from '../../../product/product.service';
import { Product } from '../../../product/product';
import { lastValueFrom } from 'rxjs';
import { ProductType } from '../../../product/product-type';

@Component({
  selector: 'app-edit-product',
  templateUrl: './edit-product.component.html',
  styleUrl: './edit-product.component.css'
})
export class EditProductComponent {
  product!: Product;
  productTypes = ProductType;
  productTypesNames = Object.keys(ProductType).filter(key => isNaN(Number(key)));
  productTypesValues = Object.values(ProductType).filter(value => !isNaN(Number(value)));
  hasError = false;
  error!: string | null;
  constructor(private route: ActivatedRoute, private router: Router, private productService: ProductService) {
    this.productService.getProductById(this.route.snapshot.queryParams['id']).subscribe(response => {
      this.product = response.data;
    })
  }

  async save() {
    this.hasError = false;
    this.error = '';

    try {
      this.product.category = Number(this.product.category);
      var req = await lastValueFrom(this.productService.updateProduct(this.product));

      this.product = req.data;

      if(req.success){
        this.router.navigate(['/admin/products']);
      }

      else{
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
