import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Pagination } from '../shared/models/pagination';
import { Brand } from '../shared/models/brand';
import { ProductType } from '../shared/models/productType';
import { map } from 'rxjs/operators';
import { ShopParams } from '../shared/models/shopParams';
import { Product } from '../shared/models/product';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  baseUrl = 'https://localhost:5001/api/';

  constructor(private http: HttpClient) { }

  getProducts(shopParams: ShopParams) {
    let params = new HttpParams();
    if(shopParams.brandId !== 0)
    {
      params = params.append('BrandId',shopParams.brandId);
    }
    if(shopParams.typeId !== 0)
    {
      params = params.append('TypeId',shopParams.typeId);
    }
    if(shopParams.search)
    {
      params = params.append('Search',shopParams.search);
    }

    params = params.append('Sort',shopParams.sort);
    params = params.append('PageIndex',shopParams.pageNumber);
    params = params.append('PageSize',shopParams.pageSize);

    const res = this.http.get<Pagination>(this.baseUrl + 'products',{observe: 'response', params})
    .pipe(
      map(
        response => {
          return response.body; 
        }
      )
    );
    return res;
  }

  getProduct(id: number) {
    const res = this.http.get<Product>(this.baseUrl + 'products/' + id);
    return res;
  }
   
  getBrands() {
    const res = this.http.get<Brand[]>(this.baseUrl + 'products/brands');
    return res;
  }

  getProductTypes() {
    const res = this.http.get<ProductType[]>(this.baseUrl + 'products/types');
    return res;
  }

}
