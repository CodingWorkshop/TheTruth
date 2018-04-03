import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material';
import { VideoService } from '../video.service';
import { SetupDialogComponent } from '../setup-dialog/setup-dialog.component';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.css'],
  providers: [VideoService]
})
export class LayoutComponent implements OnInit {

  videoList: Array<any>;
  clientList: Array<any>;

  constructor(private videoService: VideoService, public dialog: MatDialog) { }

  ngOnInit() {
    this.videoService
      .getClientIdentities()
      .subscribe(data => {
        this.clientList = data;
      });
  }

  onModelChanged(params) {
    this.videoService
      .getVideoList(params)
      .subscribe(data => {
        this.videoList = data;
      });
  }

  openSetupDialog() {
    const dialogRef = this.dialog
      .open(SetupDialogComponent, {
        width: '500px',
        data: this.clientList
      });

    dialogRef.afterClosed()
      .subscribe(result => {
        const selected = [].concat(result);

        selected.forEach(s => console.log(s));
      });
  }
}
