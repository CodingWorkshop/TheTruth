import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  constructor() {
    const connection = new signalR.HubConnection('http://192.168.43.114:5000/managementhub');

    connection.on('getonlineusers', data => {
      alert(data);
    });

    connection.start();
  }
}
