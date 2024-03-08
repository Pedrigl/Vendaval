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
  imagesLink: string = '';
  hasError = false;
  error!: string | null;

  constructor(private router: Router, private productService: ProductService) {
    productService.getAllProducts().subscribe(async response => {
      this.products = response;

      if (this.products.data.some(p => p.image == null || p.image == '')) {
        const imagesLinkResponse = await lastValueFrom(productService.CreateAuthRequestToProductImagesLink());
        this.imagesLink = imagesLinkResponse.data.fullPath;
        

        for (let p of this.products.data) {
          const imagesNamesResponse = await lastValueFrom(productService.getProductImagesNames(this.imagesLink));
          
          var names = imagesNamesResponse.objects;
          for (var i = 0; i < names.length; i++) {
            if (names[i].name.includes(p.id.toString()))
              p.image = this.imagesLink + names[i].name;
          }
        }
      }
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

      else {
        this.products.data = this.products.data.filter(p => p.id !== id);
      }
    }
    catch (error: any) {
      this.hasError = true;
      this.error = error;
    }
  }
}
