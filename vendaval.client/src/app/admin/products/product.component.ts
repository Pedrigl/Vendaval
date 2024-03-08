import { Component, OnInit } from '@angular/core';
import { ProductService } from '../../product/product.service';
import { ApiResponse } from '../../shared/common/interfaces/apiResponse';
import { Product } from '../../product/product';
import { ProductType } from '../../product/product-type';
import { Router } from '@angular/router';
import { lastValueFrom } from 'rxjs';

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

  constructor(private router: Router, private productService: ProductService) { }

  async ngOnInit() {
    try {
      this.products = await lastValueFrom(this.productService.getAllProducts());

      const loadImagesPromises = this.products.data.map(async (product) => {
        const isLinkValid = await this.checkImageLink(product.image);
        if (!isLinkValid) {
          if (!this.imagesLink) {
            const imagesLinkResponse = await lastValueFrom(this.productService.CreateAuthRequestToProductImagesLink());
            this.imagesLink = imagesLinkResponse.data.fullPath;
          }
          product.image = this.imagesLink + product.id;
        }
      });

      await Promise.all(loadImagesPromises);
    } catch (error: any) {
      this.hasError = true;
      this.error = error;
    }
  }

  async checkImageLink(imageLink: string): Promise<boolean> {
    try {
      const response = await fetch(imageLink);
      return response.ok;
    } catch (error: any) {
      console.error('Erro ao verificar o link da imagem:', error);
      return false;
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
        this.products.data = this.products.data.filter(p => p.id !== id);
      }
    } catch (error: any) {
      this.hasError = true;
      this.error = error;
    }
  }
}
