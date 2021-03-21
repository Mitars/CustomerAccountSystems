import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Customer } from '../models/customer';
import { DataService } from '../services/data.service';

@Component({
  selector: 'app-customer-list',
  templateUrl: './customer-list.component.html',
  styleUrls: ['./customer-list.component.scss'],
})
export class CustomerListComponent implements OnInit {
  visibleCustomers: Customer[] = [];
  allCustomers: Customer[] = [];
  value: string = '';

  customersLoaded = false;

  constructor(
    private customerService: DataService,
    private router: Router
  ) {}

  ngOnInit() {
    this.customerService.getCustomers().subscribe(
      (data: Customer[]) => {
        this.allCustomers = data;
        this.onFilterChange('');
        this.customersLoaded = true;
      },
      (error) => console.error(error)
    );
  }

  onFilterChange(value: string) {
    const filterText = value.toLowerCase()
    this.visibleCustomers = this.allCustomers.filter(
      (customer) =>
        customer.firstName.toLowerCase().indexOf(filterText) !== -1 ||
        customer.lastName.toLowerCase().indexOf(filterText) !== -1 ||
        (customer.firstName.toLowerCase() + ' ' + customer.lastName.toLowerCase()).indexOf(filterText) !== -1
    );
  }

  onCustomerClick(customer: Customer) {
    this.router.navigate(['/customer/' + customer.id]);
  }
}
