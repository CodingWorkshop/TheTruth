import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { LayoutComponent } from '../layout/layout.component';

@Component({
  selector: 'app-setup-dialog',
  templateUrl: './setup-dialog.component.html',
  styleUrls: ['./setup-dialog.component.css']
})
export class SetupDialogComponent implements OnInit {

  checkList: Array<any>;

  constructor(
    public dialogRef: MatDialogRef<LayoutComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.checkList = data;
  }

  ngOnInit() {
  }

  onOKClick() {
    this.dialogRef
      .close(this.checkList.filter(c => c.checked));
  }

}
