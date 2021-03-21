import { Component } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'transaction-create-dialog',
  templateUrl: './transaction-create-dialog.component.html',
  styleUrls: ['./transaction-create-dialog.component.scss'],
})
export class TransactionCreateDialog {
  formControl = new FormControl('', [
    Validators.required,
    this.notZeroValidator,
  ]);
  data = { amount: null, description: null };

  constructor(public dialogRef: MatDialogRef<TransactionCreateDialog>) {}

  onCreate() {
    this.dialogRef.close(this.data);
  }

  notZeroValidator(control: any) {
    if (
      control.value === undefined ||
      isNaN(control.value) ||
      control.value === 0
    ) {
      return { isNumber: true };
    }
    return null;
  }
}
