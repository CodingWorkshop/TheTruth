import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { MatTableDataSource } from '@angular/material';
import { SelectionModel } from '@angular/cdk/collections';
import { IVideo } from '../video.service';

@Component({
  selector: 'app-side-content',
  templateUrl: './side-content.component.html',
  styleUrls: ['./side-content.component.css']
})
export class SideContentComponent implements OnChanges {

  displayedColumns = ['select', 'position', 'displayName', 'date'];
  dataSource: MatTableDataSource<IVideo>;
  selection: SelectionModel<IVideo>;
  @Input() items: IVideo[];

  constructor() { }

  ngOnChanges(changes: SimpleChanges) {
    this.selection = new SelectionModel<IVideo>(true, []);
    this.dataSource = new MatTableDataSource(
      this.items.map((d, idx) => {
        d.position = idx + 1;
        return d;
      }));
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
