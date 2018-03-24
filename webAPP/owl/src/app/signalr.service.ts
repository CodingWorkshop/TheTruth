import { Injectable } from '@angular/core';

@Injectable()
export class SignalrService {

  connection: any;

  constructor(
  ) {
    this.connection = new signalR.HubConnection('http://192.168.43.114:5000/managementhub');
    this.connection.start();
  }

  on<T>(methodName: string, method: (T) => void) {
    this.connection.on(methodName, method);
  }
}
