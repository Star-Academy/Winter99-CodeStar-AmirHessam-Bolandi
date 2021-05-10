import {Component, OnInit} from '@angular/core';

@Component({
  selector: 'app-search-box',
  templateUrl: './search-box.component.html',
  styleUrls: ['./search-box.component.scss']
})
export class SearchBoxComponent implements OnInit {
  public advanceEnable: boolean;

  constructor() {
    this.advanceEnable = false;
  }

  ngOnInit(): void {
  }

  public getNormalValue(noramlValue: string): void {

  }

  public getButtonClick(click: any): void {
    if (!this.advanceEnable) {
      this.advanceEnable = true;
    }else {
      return;
    }
  }
}
