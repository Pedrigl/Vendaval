import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ProductService } from '../../../product/product.service';
import { Product } from '../../../product/product';
import { ProductType } from '../../../product/product-type';
import { lastValueFrom } from 'rxjs';
import { LoadingService } from '../../../shared/common/loading.service';

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
    category: ProductType.Clothing,
    createdAt: null,
    updatedAt: null
  }
  productImage!: File;
  productType = ProductType;
  productTypesNames = Object.keys(ProductType).filter(key => isNaN(Number(key)));
  productTypesValues = Object.values(ProductType).filter(value => !isNaN(Number(value)));
  hasError = false;
  error!: string | null;

  constructor(private router: Router, private loadingService: LoadingService, private productService: ProductService) {
  }

  async createProduct() {
    this.loadingService.isLoading.next(true);
    try {
      this.product.category = Number(this.product.category);
      
      await this.uploadImage();

      var res = await lastValueFrom(this.productService.createProduct(this.product));
      
      if (res.success) {
        this.hasError = false;
        this.error = '';

        this.loadingService.isLoading.next(false);
        this.router.navigate(['/admin/products']);  
      }

      else {
        this.loadingService.isLoading.next(false);
        this.hasError = true;
        this.error = res.message;
      }
    }
    catch (error: any) {
      this.loadingService.isLoading.next(false);
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
