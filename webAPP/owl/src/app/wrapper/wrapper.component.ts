import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDrawer } from '@angular/material';
import { VideoService, IClientIdentity } from '../video.service';
import { SetupDialogComponent } from '../setup-dialog/setup-dialog.component';

@Component({
  selector: 'app-wrapper',
  templateUrl: './wrapper.component.html',
  styleUrls: ['./wrapper.component.css'],
  providers: [VideoService]
})
export class WrapperComponent implements OnInit {

  videoList: Array<any>;
  videoSelected: string[];
  clientList: IClientIdentity[];
  @ViewChild('drawer') drawer: MatDrawer;

  constructor(private videoService: VideoService, public dialog: MatDialog) { }

  ngOnInit() {
    this.videoService
      .getClientIdentities()
      .subscribe(data => {
        this.clientList = data;
      });
  }

  onConditionsChanged(params) {
    this.videoService
      .getVideoList(params)
      .subscribe(data => {
        this.videoList = data;
        this.drawer.toggle();
      });
  }

  onVideoSelected(params: string[]) {
    this.videoSelected = params;
  }

  openSetupDialog() {
    const dialogRef = this.dialog
      .open(SetupDialogComponent, {
        width: '500px',
        data: this.clientList
      });

    dialogRef.afterClosed()
      .subscribe((result: Array<any>) => {
        const clientSelected = [].concat(result || []);
        clientSelected.map(c => c.id)
          .forEach(c => {
            this.setVideoToClient(c);
          });
      });
  }

  setVideoToClient(id: string) {
    this.videoService
      .setVideo({
        id: id,
        codes: this.videoSelected
      })
      .subscribe(result => {
        console.log(result);
      });
  }
}
