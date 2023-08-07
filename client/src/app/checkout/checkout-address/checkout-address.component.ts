import { Component, Input } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/account/account.service';

@Component({
  selector: 'app-checkout-address',
  templateUrl: './checkout-address.component.html',
  styleUrls: ['./checkout-address.component.scss']
})
export class CheckoutAddressComponent {
  @Input() checkoutForm: FormGroup;

  constructor(private accountService: AccountService, private toeastr: ToastrService) {}
  
  saveUserAddress() {
    this.accountService.setUserAddress(this.checkoutForm.get('addressForm').value)
      .subscribe(() => {
        this.toeastr.success('Address saved');
      }, error => {
        this.toeastr.error(error.message);
        console.log(error);
      });
  }
}
