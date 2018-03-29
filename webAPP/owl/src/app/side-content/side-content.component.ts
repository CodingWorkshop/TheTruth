import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-side-content',
  templateUrl: './side-content.component.html',
  styleUrls: ['./side-content.component.css']
})
export class SideContentComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

  onModelChanged(value) {
    console.log(value);
  }

}
