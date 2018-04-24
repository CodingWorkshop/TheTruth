import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';

@Injectable()
export class SignalrService {

  connection: any;
  isConnected: boolean;

  constructor(
  ) {
    this.connection = new signalR.HubConnection(`${environment.hubUrl}`);
    this.connection.onclose(() => {
      this.isConnected = false;
      console.warn('實時狀態斷開, 5 秒後重新連接');
      this.init();
    });

    this.init();
  }

  init(): void {
    this.connection
      .start()
      .then(() => {
        this.isConnected = true;
        console.log('實時狀態正常運作');
      })
      .catch(err => {
        this.isConnected = false;
        console.error('實時狀態異常, 5 秒後重新連接');
        setTimeout(() => {
          this.init();
        }, 5000);
      });
  }

  on<T>(methodName: string, method: (arg: T) => void) {
    this.connection.on(methodName, method);
  }
}
