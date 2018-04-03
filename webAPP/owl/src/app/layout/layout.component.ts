import { Component, OnInit } from '@angular/core';
import { VideoService } from '../video.service';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.css'],
  providers: [VideoService]
})
export class LayoutComponent implements OnInit {

  data: Array<any>;

  constructor(private videoService: VideoService) { }

  ngOnInit() {
  }

  onModelChanged(params) {
    this.videoService
      .getVideoList(params)
      .subscribe(data => {
        this.data = data;
      });
  }
}
