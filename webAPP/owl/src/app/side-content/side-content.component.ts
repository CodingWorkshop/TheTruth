import { Component, Input, Output, OnInit, OnChanges, SimpleChanges, EventEmitter } from '@angular/core';
import { MatTableDataSource } from '@angular/material';
import { SelectionModel } from '@angular/cdk/collections';
import { IVideo } from '../video.service';

@Component({
  selector: 'app-side-content',
  templateUrl: './side-content.component.html',
  styleUrls: ['./side-content.component.css']
})
export class SideContentComponent implements OnInit, OnChanges {

  displayedColumns = ['select', 'position', 'categoryName', 'name', 'date'];
  dataSource: MatTableDataSource<IVideo>;
  selection: SelectionModel<IVideo>;
  @Input() items: IVideo[];
  @Output() videoSelect: EventEmitter<string[]> = new EventEmitter<string[]>();

  constructor() { }

  ngOnInit(): void {
  }

  ngOnChanges(changes: SimpleChanges) {
    if (this.items) {
      this.selection = new SelectionModel<IVideo>(true, []);
      this.dataSource = new MatTableDataSource(
        this.items.map((d, idx) => {
          d.position = idx + 1;
          return d;
        }));
    }
  }

  toggle(row: IVideo) {
    this.selection.toggle(row);
    this.onVideoSelect();
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

    this.onVideoSelect();
  }

  onVideoSelect() {
    const selectedCode = this.dataSource.data.filter(row => this.selection.isSelected(row)).map(d => d.code);
    this.videoSelect.emit(selectedCode);
  }

}
