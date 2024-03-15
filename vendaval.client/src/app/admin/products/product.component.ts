import { Component, OnInit } from '@angular/core';
import { ProductService } from '../../product/product.service';
import { ApiResponse } from '../../shared/common/interfaces/apiResponse';
import { Product } from '../../product/product';
import { ProductType } from '../../product/product-type';
import { Router } from '@angular/router';
import { lastValueFrom } from 'rxjs';
import { LoadingService } from '../../shared/common/loading.service';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css']
})
export class ProductComponent implements OnInit {
  products!: ApiResponse<Product[]>;
  filteredProducts!: ApiResponse<Product[]>;
  productType = ProductType;
  searchTerm: string = '';
  imagesLink: string = '';
  hasError = false;
  error!: string | null;

  constructor(private router: Router, private productService: ProductService) { }

  async ngOnInit() {
    try {
      this.products = await lastValueFrom(this.productService.getAllProducts());
      this.filteredProducts = { ...this.products, data: [...this.products.data] };

    }

    catch (error: any) {
      this.hasError = true;
      this.error = error;
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
        this.hasError = false;
        this.error = '';
        this.filteredProducts.data = this.products.data.filter(p => p.id !== id);
      }
      
    } catch (error: any) {
      this.hasError = true;
      this.error = error;
    }
  }

  filterList() {
    if (this.searchTerm) {
      this.filteredProducts.data = this.products.data.filter(item => item.name.toLowerCase().includes(this.searchTerm.toLowerCase()));
    }
    else {
      this.filteredProducts.data = [...this.products.data];
    }
  }
}
