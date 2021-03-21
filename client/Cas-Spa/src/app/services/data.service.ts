import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AccountToCreate } from '../models/accountToCreate';
import { TransactionToCreate } from '../models/transactionToCreate';
import { Customer } from '../models/customer';
import { tap } from 'rxjs/operators';
import { of } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class DataService {
  constructor(private http: HttpClient) { }

  apiUrl = environment.apiUrl;
  lastFetchedUser = environment.localStorageLastFetchedUser;

  getCustomers() {
    return this.http.get(`${this.apiUrl}/customers`);
  }

  getCustomer(id: number) {
    const lastFetchedUser = JSON.parse(localStorage.getItem(this.lastFetchedUser)) as Customer;
    if (id.toString() === lastFetchedUser.id.toString()) {
      return of(lastFetchedUser);
    }

    return this.http.get(`${this.apiUrl}/customers/${id}`).pipe(tap(customer =>
      localStorage.setItem(this.lastFetchedUser, JSON.stringify(customer))));
  }

  getAccountsByCustomerId(customerId: string) {
    return this.http.get(`${this.apiUrl}/customers/${customerId}/accounts`);
  }

  getAccount(id: string) {
    return this.http.get(`${this.apiUrl}/accounts/${id}`);
  }

  createAccount(account: AccountToCreate) {
    return this.http.post(`${this.apiUrl}/accounts`, account);
  }

  getTransactions(accountId: string) {
    return this.http.get(`${this.apiUrl}/accounts/${accountId}/transactions`);
  }

  createTransaction(transaction: TransactionToCreate) {
    return this.http.post(`${this.apiUrl}/transactions`, transaction);
  }
}
