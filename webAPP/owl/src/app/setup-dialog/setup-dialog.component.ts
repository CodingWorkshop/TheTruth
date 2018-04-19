import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { WrapperComponent } from '../wrapper/wrapper.component';
import { IClientIdentity } from '../video.service';

@Component({
  selector: 'app-setup-dialog',
  templateUrl: './setup-dialog.component.html',
  styleUrls: ['./setup-dialog.component.css']
})
export class SetupDialogComponent implements OnInit {

  title: string;
  clients: IClientIdentity[];
  selectedOptions: number[];

  constructor(
    public dialogRef: MatDialogRef<WrapperComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.title = data.title;
    this.clients = data.list || [];
  }

  ngOnInit() {
  }

  onNgModelChange($event: Event) {
  }

  onOKClick() {
    this.dialogRef
      .close(this.selectedOptions);
  }
}
