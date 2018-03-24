import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = '貓頭鷹管理後台';

  constructor() {
    const connection = new signalR.HubConnection('http://localhost:5000/videohub');

    connection.on('getonlineusers', data => {
      alert(data);
    });

    connection.start();
  }
}
