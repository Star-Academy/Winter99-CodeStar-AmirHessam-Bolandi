import {Component, OnInit} from '@angular/core';

@Component({
  selector: 'app-search-box',
  templateUrl: './search-box.component.html',
  styleUrls: ['./search-box.component.scss']
})
export class SearchBoxComponent implements OnInit {
  public advanceEnable: boolean;
  public normalValue: string;
  public plusValue: string;
  public minusValue: string;
  public buttonLabel: string;

  constructor() {
    this.advanceEnable = false;
    this.buttonLabel = 'جست و جوی پیشرفته';
  }

  ngOnInit(): void {
  }

  public getNormalValue(normalValue: string): void {
    this.normalValue = normalValue;
  }

  public getMinusValue(minusValue: string): void {
    this.minusValue = minusValue;
  }

  public getPlusValue(plusValue: string): void {
    this.plusValue = plusValue;
  }

  public getButtonClick(click: any): void {
    if (!this.advanceEnable) {
      this.advanceEnable = true;
      this.buttonLabel = 'جست و جو';
    } else {
      console.log(this.normalValue + ' - ' + this.minusValue + ' ' + this.plusValue);
    }
  }
}
