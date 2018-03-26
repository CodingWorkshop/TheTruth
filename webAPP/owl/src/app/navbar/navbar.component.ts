import { Component, OnInit } from '@angular/core';

import { SignalrService } from '../signalr.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  onlineUsers: number;

  constructor(private signalrService: SignalrService) {
  }

  ngOnInit() {
    this.onlineUsers = 0;

    this.signalrService
      .on('getonlineusers', val => {
        this.onlineUsers = val;
      });
  }
}
