import { Routes } from '@angular/router';
import { AccountDetailComponent } from './account-detail/account-detail.component';
import { CustomerDetailComponent } from './customer-detail/customer-detail.component';
import { CustomerListComponent } from './customer-list/customer-list.component';
import { LandingComponent } from './landing/landing.component';

export const appRoutes: Routes = [
  { path: '', component: LandingComponent },
  { path: 'customers', component: CustomerListComponent },
  { path: 'customer/:customerId/account/:id', component: AccountDetailComponent },
  { path: 'customer/:id', component: CustomerDetailComponent },
  { path: '**', redirectTo: '', pathMatch: 'full' }
]
