import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DataService } from '../services/data.service';
import { MatDialog } from '@angular/material/dialog';
import { Customer } from '../models/customer';
import { Account } from '../models/account';
import { AccountCreateDialog } from './account-create-dialog/account-create-dialog.component';
import { MatSnackBar } from '@angular/material/snack-bar';
import { SnackbarService } from '../services/snackbar.service';

@Component({
  selector: 'app-customer-detail',
  templateUrl: './customer-detail.component.html',
  styleUrls: ['./customer-detail.component.scss'],
})
export class CustomerDetailComponent {
  customer: Customer = {} as Customer;
  accounts: Account[] = [];

  balance: number = 0;

  isCustomerLoading = false;
  isAccountsLoading = false;
  isAccountCreating = false;
  isErrorLoading = false;

  constructor(
    private customerService: DataService,
    private route: ActivatedRoute,
    private dialog: MatDialog,
    private snackbar: SnackbarService
  ) {
    this.isCustomerLoading = true;
    this.isAccountsLoading = true;

    this.route.params.subscribe((params) => {
      this.customerService.getCustomer(params['id']).subscribe((data) => {
        this.customer = data as Customer;
        this.customer.dateOfBirth = new Date(this.customer.dateOfBirth);
      }, (error) => {
        console.error(error);
        this.isErrorLoading = true;
        this.snackbar.error('Failed to load the customer');
      }).add(() => this.isCustomerLoading = false);

      this.customerService
        .getAccountsByCustomerId(params['id'])
        .subscribe((data: Account[]) => {
          data.forEach((account) => {
            account.created = new Date(account.created);
            this.accounts.push(account);
          });
        }, (error) => {
          console.error(error);
          this.isErrorLoading = true;
          this.snackbar.error('Failed to load the accounts');
        })
        .add(() => this.isAccountsLoading = false);
    })
  }

  getTotalBalance() {
    return this.accounts.reduce(
      (sum, account) => sum + this.getBalance(account),
      0
    );
  }

  getBalance(account: Account) {
    return account.transactions.reduce(
      (sum, transaction) => sum + transaction.amount,
      0
    );
  }

  openCreateDialog(): void {
    this.dialog
      .open(AccountCreateDialog, {
        width: '380px',
        height: '270px',
      })
      .afterClosed()
      .subscribe((result) => {
        if (!result) {
          return;
        }

        this.isAccountCreating = true;
        const accountToCreate = { ...result, customerId: this.customer.id };

        this.customerService
          .createAccount(accountToCreate)
          .subscribe((data: Account) => {
            this.accounts.push(data);
            this.snackbar.success('Successfully created the account');
          },
          (error) => {
            console.error(error)
            this.snackbar.error('Failed creating the account');
          })
          .add(() => this.isAccountCreating = false);
      });
  }
}
