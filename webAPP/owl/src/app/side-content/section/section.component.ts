import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-section',
  templateUrl: './section.component.html',
  styleUrls: ['./section.component.css']
})
export class SectionComponent implements OnInit {
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
