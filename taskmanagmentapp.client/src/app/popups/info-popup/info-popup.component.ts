import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-info-popup',
  standalone: true,
  imports: [CommonModule, MatDialogModule],
  templateUrl: './info-popup.component.html',
  styleUrl: './info-popup.component.css'
})
export class InfoPopupComponent {
  constructor(
    public dialogRef: MatDialogRef<InfoPopupComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { message: string }
  ) {}
}
