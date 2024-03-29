import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { OrdersDetailedComponent } from './orders-detailed/orders-detailed.component';
import { OrdersComponent } from './orders.component';

const routes: Routes = [
  { path: '', component: OrdersComponent },
  { path: ':id', component: OrdersDetailedComponent, data: { breadcrumb: { alias: 'OrderDetailed' } } }
]

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class OrdersRoutingModule { }