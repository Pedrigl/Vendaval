import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { LoadingService } from '../shared/common/loading.service';
import { ProductService } from './product.service';
import { Product } from './product';
import { ProductType } from './product-type';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrl: './products.component.css'
})
export class ProductsComponent {
  products!: Product[];
  productTypes = ProductType;
  productTypesNames = Object.keys(ProductType).filter(key => isNaN(Number(key)));
  productTypesValues = Object.values(ProductType).filter(value => !isNaN(Number(value)));
  fiteredProductTypes!: ProductType;
  filteredProducts!: Product[];
  nameFilter: string = '';
  initialPriceFilter: number = 0;
  maxPriceFilter: number = 0;


  constructor(private router: Router, private productService: ProductService) {
    
    productService.getAllProducts().subscribe(response => {
      this.products = response.data;
      this.filteredProducts = [...this.products];
    });
  }

  applyNameFilter() {
    if (this.nameFilter) {
      this.filteredProducts = this.products.filter(p => p.name.toLowerCase().includes(this.nameFilter.toLowerCase()));
    }
    else {
      this.filteredProducts = [...this.products];
    }
  }

  applyPriceFilter() {
    if (this.initialPriceFilter > 0 && this.maxPriceFilter > 0) {
      this.filteredProducts = this.products.filter(p => p.price >= this.initialPriceFilter && p.price <= this.maxPriceFilter);
    }
    else {
      this.filteredProducts = [...this.products];
    }
  }

  applyCategoryFilter() {
    if (Number(this.fiteredProductTypes) != -1) {
      this.filteredProducts = this.products.filter(p => p.category == this.fiteredProductTypes);
    }
    else {
      this.filteredProducts = [...this.products];
    }
  }

}
