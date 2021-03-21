import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { Account } from '../models/account';
import { DataService } from '../services/data.service';
import { Customer } from '../models/customer';
import { TransactionToCreate } from '../models/transactionToCreate';
import { Transaction } from '../models/transaction';
import { TransactionCreateDialog } from './transaction-create-dialog/transaction-create-dialog.component';

@Component({
  selector: 'app-account-detail',
  templateUrl: './account-detail.component.html',
  styleUrls: ['./account-detail.component.scss'],
})
export class AccountDetailComponent {
  account: Account;
  customer: Customer;

  isCustomerLoading = false;
  isAccountLoading = false;
  isTransactionCreating = false;

  constructor(
    private customerService: DataService,
    private route: ActivatedRoute,
    private dialog: MatDialog
  ) {
    this.isCustomerLoading = true;
    this.isAccountLoading = true;

    this.route.params.subscribe((params) => {
      this.customerService
        .getCustomer(params['customerId'])
        .subscribe((data) => {
          this.customer = data as Customer;
          this.isCustomerLoading = false;
        });

      this.customerService.getAccount(params['id']).subscribe((data) => {
        this.account = data as Account;
        this.account.created = new Date(this.account.created);
        this.isAccountLoading = false;
      });
    });
  }

  getBalance() {
    return this.account?.transactions?.reduce(
      (sum, transaction) => sum + transaction.amount,
      0
    );
  }

  openCreateDialog(): void {
    this.dialog
      .open(TransactionCreateDialog, {
        width: '420px',
        height: '260px',
      })
      .afterClosed()
      .subscribe((result) => {
        if (!result) {
          return;
        }

        this.isTransactionCreating = true;
        const transactionToCreate = { ...result, accountId: this.account.id } as TransactionToCreate;
        this.customerService
          .createTransaction(transactionToCreate)
          .subscribe((data: Transaction) => this.account.transactions.unshift(data))
          .add(() => this.isTransactionCreating = false);
      });
  }
}
