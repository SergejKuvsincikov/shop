import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Pagination } from '../shared/models/pagination';
import { Brand } from '../shared/models/brand';
import { ProductType } from '../shared/models/productType';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  baseUrl = 'https://localhost:5001/api/';

  constructor(private http: HttpClient) { }

  getProducts(brandId?: number, typeId?: number) {
    let params = new HttpParams();
    if(brandId)
    {
      params = params.append('BrandId',brandId);
    }
    if(typeId)
    {
      params = params.append('TypeId',typeId);
    }
    
    console.log(brandId + " " + typeId);

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

  getBrands() {
    const res = this.http.get<Brand[]>(this.baseUrl + 'products/brands');
    return res;
  }

  getProductTypes() {
    const res = this.http.get<ProductType[]>(this.baseUrl + 'products/types');
    return res;
  }

}
