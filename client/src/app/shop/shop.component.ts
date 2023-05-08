import { Component, OnInit } from '@angular/core';
import { Product } from '../shared/models/product';
import { ShopService } from './shop.service';
import { Brand } from '../shared/models/brand';
import { ProductType } from '../shared/models/productType';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})
export class ShopComponent implements OnInit{
  products: Product[];
  brands: Brand[];
  productTypes: ProductType[];
  brandIdSelected: number = 0;
  typeIdSelected: number = 0;

  constructor(private shopService: ShopService ) {}
  
  
  ngOnInit(): void {
    this.getProducts();
    this.getBrands();
    this.getProductTypes();
  }

  getProducts(): void {
    console.log(this.brandIdSelected + " " + this.typeIdSelected);
    this.shopService.getProducts(this.brandIdSelected,this.typeIdSelected).subscribe({next: response => 
      {
        this.products = response.data;
      },
      error: error => {
        console.log(error);
      }
    })
  }

  getBrands(): void {
    this.shopService.getBrands().subscribe({next: response => 
      {
        this.brands = [{id:0, name : 'All'}, ...response];
      },
      error: error => {
        console.log(error);
      }
    })
  }

  getProductTypes(): void {
    this.shopService.getProductTypes().subscribe({next: response => 
      {
        this.productTypes = [{id:0, name : 'All'}, ...response];
      },
      error: error => {
        console.log(error);
      }
    })
  }
  
  onBrandSelected(brandId: number) {
    console.log(brandId);
    this.brandIdSelected = brandId;
    this.getProducts();
  }
  
  onTypeSelected(typeId: number) {
    console.log(typeId);
    this.typeIdSelected =  typeId;
    this.getProducts();
  }
}
