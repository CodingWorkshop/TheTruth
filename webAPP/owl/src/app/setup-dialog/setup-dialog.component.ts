import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatCheckboxChange } from '@angular/material';
import { WrapperComponent } from '../wrapper/wrapper.component';
import { IClientIdentity } from '../video.service';

@Component({
  selector: 'app-setup-dialog',
  templateUrl: './setup-dialog.component.html',
  styleUrls: ['./setup-dialog.component.css']
})
export class SetupDialogComponent implements OnInit {

  checkList: IClientIdentity[];

  constructor(
    public dialogRef: MatDialogRef<WrapperComponent>,
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

  onGridTitleClicked(item: any) {
    item.checked = !item.checked;
  }

  onCheckBoxClicked($event: Event) {
    $event.preventDefault();
  }
}
