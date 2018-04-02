import { environment } from '../environments/environment';

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class VideoService {

  constructor(private http: HttpClient) { }

  getVideoList(params) {
    return this.http.get<IVideo[]>(`${environment.apiUrl}/Video/SearchVideos`, { params: params });
  }

  getCategories() {
    return this.http.get<ICategory[]>(`${environment.apiUrl}/Video/GetCategories`);
  }

  getClientIdentities() {
    return this.http.get<IClientIdentity[]>(`${environment.apiUrl}/Video/getClientIdentities`);
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

export interface ICategory {
  id: number;
  displayName: string;
  checked: boolean;
}

export interface IClientIdentity {
  id: number;
  isActive: boolean;
}
