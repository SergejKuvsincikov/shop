import { HttpBackend, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Basket, IBasket, IBasketItem } from '../shared/models/basket';
import { Product } from '../shared/models/product';

@Injectable({
  providedIn: 'root'
})
export class BasketService {
  baseUrl = environment.apiUrl;
  private basketSource = new BehaviorSubject<IBasket>(null);
  basket$ = this.basketSource.asObservable();

  constructor(private http: HttpClient) { }

  getBasket(id: string) {
    return this.http.get(this.baseUrl +'basket?id' + id).pipe(
      map((basket: IBasket)  => 
      {
        this.basketSource.next(basket);
      })
    );
  }

  setBasket(basket: IBasket)
  {
    return this.http.post(this.baseUrl + 'basket',basket).subscribe((response:IBasket) => 
    {
      this.basketSource.next(response);
    },
    error => {
      console.log(error);
    })
  }

  getCurrentBasketValue() {
    return this.basketSource.value;
  }

  addItemToBasket(item: Product, quantity =1)
  {
    const itemToAdd: IBasketItem = this.mapProductToBasketItem(item, quantity);
    const basket= this.getCurrentBasketValue() ?? this.CreateBasket();
    basket.items = this.addOrUpdateItem(basket.items,itemToAdd,quantity);
    this.setBasket(basket);
  }
  addOrUpdateItem(items: IBasketItem[], itemToAdd: IBasketItem, quantity: number): IBasketItem[] {
    const index = items.findIndex(i => i.id === itemToAdd.id);
    if(index === -1)
    {
      itemToAdd.quantity = quantity;
      items.push;
    }
    else
    {
      items[index].quantity += quantity;
    }
    return items;
  }

  private CreateBasket(): IBasket {
    const basket = new Basket();
    localStorage.setItem('basket_id',basket.id);
    return basket;
  }

  mapProductToBasketItem(item: Product, quantity: number): IBasketItem {
    return {
      id: item.id.toString(),
      productName: item.name,
      price: item.price,
      pictureUrl: item.pictureUrl,
      quantity,
      brand: item.productBrand,
      type: item.productType
    };
  }
}
