import {v4 as uuidv4} from 'uuid'

export interface IBasket {
    id: string
    items: IBasketItem[]
    clientSecret?: string
    paymentIntentId?: string
    deliveryMethodId?: number
    shippingPrice?: number
  }
  
  export interface IBasketItem {
    id: string
    productName: string
    price: number
    quantity: number
    pictureUrl: string
    brand: string
    type: string
  }

  export class Basket implements IBasket {
      id = uuidv4();
      items: IBasketItem[] = [];
    }

  export interface IBasketTotal {
    shipping: number;
    subtotal: number;
    total: number;
  }

   