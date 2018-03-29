import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-side-content',
  templateUrl: './side-content.component.html',
  styleUrls: ['./side-content.component.css']
})
export class SideContentComponent implements OnInit {
  startDate: FormControl;
  endDate: FormControl;

  constructor() { }

  ngOnInit() {
    this.startDate = new FormControl(new Date());
  }

}
