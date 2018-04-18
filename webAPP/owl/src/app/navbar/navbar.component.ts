import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDrawer } from '@angular/material';

import { VideoService } from '../video.service';
import { SetupDialogComponent } from '../setup-dialog/setup-dialog.component';
import { SignalrService } from '../signalr.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  onlineUsers: number;

  constructor(
    private signalrService: SignalrService,
    public dialog: MatDialog,
    private videoService: VideoService) {
  }

  ngOnInit() {
    this.onlineUsers = 0;

    this.signalrService
      .on('getOnlineUsers', val => {
        this.onlineUsers = val;
      });
  }

  onClick() {
    const dialogRef = this.dialog
      .open(SetupDialogComponent, {
        width: '500px',
        data: {
          title: '選擇停止播放之 Client', list: [
            { id: 104, isActive: true },
            { id: 128, isActive: true }
          ]
        }
      });

    dialogRef.afterClosed()
      .subscribe((result: Array<any>) => {
        const clientSelected = [].concat(result || []);
        clientSelected.map(c => c.id)
          .forEach(c => {
            this.cleanVideo(c);
          });
      });
  }

  cleanVideo(id: string) {
    this.videoService
      .cleanVideo(id)
      .subscribe(result => {
        console.log(result);
      });
  }
}
