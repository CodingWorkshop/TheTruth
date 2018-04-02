import { environment } from '../environments/environment';

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class VideoService {

  constructor(private http: HttpClient) { }

  getVideoList() {
    return this.http.get<IVideo[]>(`${environment.apiUrl}/Video/getvideolist`);
  }
}

export interface IVideo {
  position: number;
  id: string;
  name: string;
  date: string;
  displayName: string;
  code: string;
}
