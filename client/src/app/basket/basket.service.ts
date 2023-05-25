import { HttpBackend, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Basket, IBasket, IBasketItem, IBasketTotal } from '../shared/models/basket';
import { Product } from '../shared/models/product';

@Injectable({
  providedIn: 'root'
})
export class BasketService {
  baseUrl = environment.apiUrl;
  private basketSource = new BehaviorSubject<IBasket>(null);
  basket$ = this.basketSource.asObservable();
  private basketTotalSource = new BehaviorSubject<IBasketTotal>(null);
  basketTotal$ = this.basketTotalSource.asObservable();

  constructor(private http: HttpClient) { }

  getBasket(id: string) {
    console.log(this.baseUrl +'basket?id=' + id);
    return this.http.get(this.baseUrl +'basket?id=' + id).pipe(
      map((basket: IBasket)  => 
      {
        this.basketSource.next(basket);
        this.calculateItems();
      })
    );
  }

  setBasket(basket: IBasket)
  {
    console.log("basket = " +basket);
    console.log(this.baseUrl + 'basket');
    return this.http.post(this.baseUrl + 'basket',basket).subscribe((response:IBasket) => 
    {
      this.basketSource.next(response);
      this.calculateItems();
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
    console.log(items);
    const index = items.findIndex(i => i.id === itemToAdd.id);
    if(index === -1)
    {
      itemToAdd.quantity = quantity;
      items.push(itemToAdd);
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

  private calculateItems() {
    const basket = this.getCurrentBasketValue();
    const shipping = 0;
    const subtotal = basket.items.reduce((a,b) => (b.price * b.quantity) + a, 0);
    const total = subtotal + shipping;
    this.basketTotalSource.next({shipping,total,subtotal});
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
