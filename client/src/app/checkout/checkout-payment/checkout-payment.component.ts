import { AfterViewInit, Component, ElementRef, Input, OnDestroy, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { BasketService } from 'src/app/basket/basket.service';
import { CheckoutService } from '../checkout.service';
import { ToastrService } from 'ngx-toastr';
import { IBasket } from 'src/app/shared/models/basket';
import { IOrder } from 'src/app/shared/models/orders';
import { NavigationExtras, Router } from '@angular/router';

declare var Stripe;

@Component({
  selector: 'app-checkout-payment',
  templateUrl: './checkout-payment.component.html',
  styleUrls: ['./checkout-payment.component.scss']
})
export class CheckoutPaymentComponent implements AfterViewInit, OnDestroy {
  @Input() checkoutForm: FormGroup;
  @ViewChild('cardNumber') cardNumberElement?: ElementRef;
  @ViewChild('cardExpiry') cardExpiryElement?: ElementRef;
  @ViewChild('cardCvc') cardCvcElement?: ElementRef;
  stripe: any;
  cardNumber: any;
  cardExpiry: any;
  cardCvc: any;
  cardErrors: any;
  cardHandler = this.onChange.bind(this);
  

  constructor(private basketService: BasketService, private checkoutService: CheckoutService, 
    private toastr: ToastrService, private router: Router) {

  }
  ngOnDestroy(): void {
    this.cardNumber.destroy(); 
    this.cardExpiry.destroy();
    this.cardCvc.destroy();
  }
  ngAfterViewInit(): void {
    this.stripe = Stripe('pk_test_51NeyO0Kz7MZfJyPE4DGc9JViflc42wH39j1kmJGEudpdCU9o1OqFjNpvJinMK037vBCo9KX7Dcc0KZRXzIcopxjN00jPzWaYj7');
    const elements = this.stripe.elements();

    this.cardNumber = elements.create('cardNumber');
    this.cardNumber.mount(this.cardNumberElement.nativeElement);
    this.cardNumber.addEventListener('change',this.cardHandler);

    this.cardExpiry = elements.create('cardExpiry');
    this.cardExpiry.mount(this.cardExpiryElement.nativeElement);
    this.cardExpiry.addEventListener('change',this.cardHandler);

 
    this.cardCvc = elements.create('cardCvc');
    this.cardCvc.mount(this.cardCvcElement.nativeElement); 
    this.cardCvc.addEventListener('change',this.cardHandler);
  }
  
  onChange({error}) {
    if (error) 
    {
      this.cardErrors = error.message;
    }
    else
    {
      this.cardErrors = null;
    }
  }

  submitOrder() {
      const basket = this.basketService.getCurrentBasketValue();
      const orderToCrete = this.getOrderToCreate(basket);
      this.checkoutService.createOrder(orderToCrete).subscribe((order: IOrder) => {
        this.toastr.success('Order Created Successfully');
        this.basketService.deleteLocalBasket(basket.id);
        const navigationExtras: NavigationExtras = {state: order}; 
        this.router.navigate(['checkout/success'],navigationExtras);
      }, error => {
        this.toastr.error(error.message);
        console.log(error);
      } )
  }
  private  getOrderToCreate(basket: IBasket) {
    return {
      baskerId: basket.id,
      deliveryMethodId: +this.checkoutForm.get('deliveryForm').get('deliveryMethod').value,
      shipToAddress: this.checkoutForm.get('addressForm').value
    }
  }

}
