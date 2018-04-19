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
  @ViewChild('drawer') drawer: MatDrawer;

  constructor(private videoService: VideoService, public dialog: MatDialog) { }

  ngOnInit() {
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

  getClientList(callback: (arg: any) => void) {
    return this.videoService
      .getClientIdentities()
      .subscribe(data => {
        callback(data);
      });
  }

  openSetupDialog() {
    const callback = (data: IClientIdentity[]) => {
      const dialogRef = this.dialog
        .open(SetupDialogComponent, {
          width: '500px',
          data: { title: '選擇要播放之 Client', list: data }
        });

      dialogRef.afterClosed()
        .subscribe((result: Array<any>) => {
          const clientSelected = [].concat(result || []);
          clientSelected.forEach(c => {
            this.setVideoToClient(c);
          });
        });
    };

    this.getClientList(callback);
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
