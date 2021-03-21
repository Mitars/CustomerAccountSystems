import { Component } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'account-create-dialog',
  templateUrl: './account-create-dialog.component.html',
  styleUrls: ['./account-create-dialog.component.scss'],
})
export class AccountCreateDialog {
  formControl = new FormControl('', [Validators.required]);
  data = { name: null, initialCredit: null };

  constructor(public dialogRef: MatDialogRef<AccountCreateDialog>) {}

  onCreate() {
    this.dialogRef.close(this.data);
  }
}
