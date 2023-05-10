import { Component, OnInit } from '@angular/core';
import { Product } from '../shared/models/product';
import { ShopService } from './shop.service';
import { Brand } from '../shared/models/brand';
import { ProductType } from '../shared/models/productType';
import { ShopParams } from '../shared/models/shopParams';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})
export class ShopComponent implements OnInit{
  products: Product[];
  brands: Brand[];
  productTypes: ProductType[];
  shopParams = new ShopParams();
  totalCount: number;

  sortOptions = [
    {name: 'Alphabetical', value: 'name'},
    {name: 'Price: Low to high', value: 'PriceAsc'},
    {name: 'Price: High to low', value: 'PriceDesc'},
  ];

  constructor(private shopService: ShopService ) {}
  
  
  ngOnInit(): void {
    this.getProducts();
    this.getBrands();
    this.getProductTypes();
  }

  getProducts(): void {
    console.log(this.sortOptions);
    this.shopService.getProducts(this.shopParams).subscribe({next: response => 
      {
        this.products = response.data;
        this.shopParams.pageNumber = response.pageIndex;
        this.shopParams.pageSize = response.pageSize;
        this.totalCount = response.count;
      },
      error: error => {
        console.log(error);
        this.products = null;;
        this.totalCount = 0;
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
    this.shopParams.brandId = brandId;
    this.getProducts();
  }
  
  onTypeSelected(typeId: number) {
    this.shopParams.typeId =  typeId;
    this.getProducts();
  }

  onSortSelected(sort: string)
  {
    this.shopParams.sort = sort;
    this.getProducts();
  }

  onPageChanged(event: any)
  {
    console.log(event.page);
    this.shopParams.pageNumber = event.page;
    this.getProducts();
  }
}
