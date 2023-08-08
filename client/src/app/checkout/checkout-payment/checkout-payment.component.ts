import { Component, Input } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { BasketService } from 'src/app/basket/basket.service';
import { CheckoutService } from '../checkout.service';
import { ToastrService } from 'ngx-toastr';
import { IBasket } from 'src/app/shared/models/basket';
import { IOrder } from 'src/app/shared/models/orders';
import { NavigationExtras, Router } from '@angular/router';

@Component({
  selector: 'app-checkout-payment',
  templateUrl: './checkout-payment.component.html',
  styleUrls: ['./checkout-payment.component.scss']
})
export class CheckoutPaymentComponent {
  @Input() checkoutForm: FormGroup;

  constructor(private basketService: BasketService, private checkoutService: CheckoutService, 
    private toastr: ToastrService, private router: Router) {

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
