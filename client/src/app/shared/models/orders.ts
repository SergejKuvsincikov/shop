import { IAddress } from "./address"

export interface IOrderToCreate {
    baskerId: string
    deliveryMethodId: number
    shipToAddress: IAddress
  }
  
  export interface IOrder {
    id: number
    buyerEmail: string
    orderDate: string
    shipToAddress: IAddress
    deliveryMethod: string
    shippingPrice: string
    orderItems: IOrderItem[]
    subtotal: number
    status: string
    total: number
  }
  
  export interface IOrderItem {
    productId: number
    productName: string
    pictureUrl: string
    price: number
    quantity: number
  }
  