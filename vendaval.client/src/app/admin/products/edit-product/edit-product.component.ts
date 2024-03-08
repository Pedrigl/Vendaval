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
  productImage!: File;
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
    this.productImage = (document.getElementById('image') as HTMLInputElement).files![0];
    
    try {
      this.product.category = Number(this.product.category);

      await this.updateImage();

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
      this.error = error.title;
    }
      
    }

    private async updateImage() {
        if (this.productImage != null) {
            var upload = await lastValueFrom(this.productService.uploadImage(this.product.name, this.productImage));
            
            var getLink = await lastValueFrom(this.productService.CreateAuthRequestToProductImagesLink());
            
            var names = await lastValueFrom(this.productService.getProductImagesNames(getLink.data.fullPath));

            var productImage = names.objects.filter((obj: any) => obj.name == `ProductName${this.product.name}`);
            this.product.image = getLink.data.fullPath + productImage[0].name;
        }
    }
}
