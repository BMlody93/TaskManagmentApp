import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';

import { UserTypePipe } from '../pipes/user-pipes';

import { User } from '../models/user.model';

import { ConfirmPopupComponent } from '../popups/confirm-popup/confirm-popup.component';

@Component({
  selector: 'app-user-selector',
  standalone: true,
  imports: [CommonModule, FormsModule, UserTypePipe],
  templateUrl: './user-selector.component.html',
  styleUrls: ['./user-selector.component.css']
})
export class UserSelectorComponent {
  @Input() users: User[] = [];  // Users list passed as input
  @Output() userSelected = new EventEmitter<User>();  // Event to emit selected user

  selectedUser: User | null = null  // Selected user
  previousUser: User | null = null

  constructor(
    private dialog: MatDialog) { }

  // when user is selected ask for confirmation from user, and emits event with new user
  onUserChange() {
    if (this.selectedUser) {
      if (this.previousUser) {
        const dialogRef = this.dialog.open(ConfirmPopupComponent, {
          width: '300px',
          data: { message: 'All unsaved changes will be discarded. Are you sure you want to change user?' }
        });

        dialogRef.afterClosed().subscribe((confirmed) => {
          if (confirmed) {
            // User confirmed, update previousValue
            this.previousUser = this.selectedUser;
            this.userSelected.emit(this.selectedUser!);  // Emit the selected user when changed
          } else {
            // User canceled, restore previous value
            this.selectedUser = this.previousUser;
          }
        });
      } else {
        this.previousUser = this.selectedUser;
        this.userSelected.emit(this.selectedUser!);  // Emit the selected user when changed
      }
    }    
  }
}
