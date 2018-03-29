import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-side-bar',
  templateUrl: './side-bar.component.html',
  styleUrls: ['./side-bar.component.css']
})
export class SideBarComponent implements OnInit {
  startDate: FormControl;
  endDate: FormControl;

  @Output() modelChange: EventEmitter<ISearchCondtion> = new EventEmitter<ISearchCondtion>();

  constructor() { }

  ngOnInit() {
    this.assignInitDate();
  }

  search() {
    this.modelChange.emit({
      startDate: this.startDate.value ? this.startDate.value.toISOString() : null,
      endDate: this.endDate.value ? this.endDate.value.toISOString() : null
    });
  }

  assignInitDate() {
    this.startDate = new FormControl();
    this.endDate = new FormControl();
  }

  clearAll() {
    this.assignInitDate();
  }
}

interface ISearchCondtion {
  startDate: string;
  endDate: string;
}
