import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';

@Injectable()
export class SignalrService {

  connection: any;

  constructor(
  ) {
    this.connection = new signalR.HubConnection(`${environment.hubUrl}`);
    this.connection.start();
  }

  on<T>(methodName: string, method: (arg: T) => void) {
    this.connection.on(methodName, method);
  }
}
