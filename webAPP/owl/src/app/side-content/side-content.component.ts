import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material';
import { SelectionModel } from '@angular/cdk/collections';
import { VideoService, IVideo } from '../video.service';

@Component({
  selector: 'app-side-content',
  templateUrl: './side-content.component.html',
  styleUrls: ['./side-content.component.css'],
  providers: [VideoService]
})
export class SideContentComponent implements OnInit {

  displayedColumns = ['select', 'position', 'displayName', 'date'];
  dataSource: MatTableDataSource<IVideo>;
  selection = new SelectionModel<IVideo>(true, []);

  constructor(private videoService: VideoService) { }

  ngOnInit() {
  }

  onModelChanged(params) {
    this.videoService
      .getVideoList(params)
      .subscribe(data => {
        this.dataSource = new MatTableDataSource(
          data.map((d, idx) => {
            d.position = idx + 1;
            return d;
          }));
      });
  }

  /** Whether the number of selected elements matches the total number of rows. */
  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows;
  }

  /** Selects all rows if they are not all selected; otherwise clear selection. */
  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.dataSource.data.forEach(row => this.selection.select(row));
  }

}
