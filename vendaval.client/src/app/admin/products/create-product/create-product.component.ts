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
  productImage!: File;
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
      var res = await lastValueFrom(this.productService.createProduct(this.product));

      if (res.success) {
        this.hasError = false;
        this.error = '';
        this.product = res.data;
        await this.uploadImage();
        var req = await lastValueFrom(this.productService.updateProduct(this.product));

        if (req.success)
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

  private async uploadImage() {
    this.productImage = (document.getElementById('image') as HTMLInputElement).files![0];
    var imageName = this.productImage.name;
    if (this.productImage != null) {
      var upload = await lastValueFrom(this.productService.uploadImage(this.productImage));
      
      var getLink = await lastValueFrom(this.productService.CreateAuthRequestToProductImagesLink());
      this.product.image = getLink.data.fullPath + imageName;
    }
  }
}
