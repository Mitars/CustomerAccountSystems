import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})
export class SnackbarService {

  constructor(
    private _snackBar: MatSnackBar) {
  }

  error(message: string) {
    return this.openSnackBar(message, 'snackbar-error');
  }

  success(message: string) {
    return this.openSnackBar(message, 'snackbar-success');
  }

  info(message: string) {
    return this.openSnackBar(message, 'snackbar-info');
  }

  private openSnackBar(message: string, panelClass: string) {
    return this._snackBar.open(message, 'Close', {
      horizontalPosition: 'end',
      verticalPosition: 'bottom',
      duration: 3000,
      panelClass: [panelClass]});
  }
}
