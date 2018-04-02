import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MAT_MOMENT_DATE_FORMATS, MomentDateAdapter } from '@angular/material-moment-adapter';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core';
import * as _moment from 'moment';
const moment = _moment;

@Component({
  selector: 'app-side-bar',
  templateUrl: './side-bar.component.html',
  styleUrls: ['./side-bar.component.css'],
  providers: [
    { provide: DateAdapter, useClass: MomentDateAdapter, deps: [MAT_DATE_LOCALE] },
    { provide: MAT_DATE_FORMATS, useValue: MAT_MOMENT_DATE_FORMATS }
  ]
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
      startDate: this.startDate.value ? moment(this.startDate.value).format('YYYY-MM-DD') : null,
      endDate: this.endDate.value ? moment(this.endDate.value).format('YYYY-MM-DD') : null
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

export interface ISearchCondtion {
  startDate: string;
  endDate: string;
}
