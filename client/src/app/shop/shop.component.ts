import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
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
  @ViewChild('search',{static: false}) searchTerm: ElementRef;
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
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }
  
  onTypeSelected(typeId: number) {
    this.shopParams.typeId =  typeId;
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }

  onSortSelected(sort: string)
  {
    this.shopParams.sort = sort;
    this.getProducts();
  }

  onPageChanged(event: any)
  {
    console.log(this.shopParams.pageNumber + "   " + event);
    if(this.shopParams.pageNumber !== event)
    {

      this.shopParams.pageNumber = event;
      this.getProducts();
    }
  }

  onSearch() {
    this.shopParams.search = this.searchTerm.nativeElement.value;
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.shopParams = new ShopParams();
    this.getProducts();
  }
}
