import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';

@Injectable()
export class SignalrService {

  connection: any;

  constructor(
  ) {
    this.connection = new signalR.HubConnection(`${environment.apiUrl}managementhub`);
    this.connection.start();
  }

  on<T>(methodName: string, method: (T) => void) {
    this.connection.on(methodName, method);
  }
}
