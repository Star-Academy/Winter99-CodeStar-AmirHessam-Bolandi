import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {HttpService} from '../../../services/http.service';

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

  @Output()
  public outPutResults: EventEmitter<string[]> = new EventEmitter<string[]>();


  constructor(private httpService: HttpService) {
    this.advanceEnable = false;
    this.buttonLabel = 'جست و جوی پیشرفته';
    const initResponse = httpService.initElastic();
  }

  ngOnInit(): void {
  }

  public getNormalValue(normalValue: string): void {
    this.normalValue = normalValue;
    if (!this.advanceEnable) {
      this.searchQuery(this.normalValue);
    }
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
      this.searchQuery(this.normalValue, this.plusValue, this.minusValue);
    }
  }

  async searchQuery(normQuery: string = '', plusQuery: string = '', minusQuery: string = ''): Promise<void> {
    const response = await this.httpService.getQueryResult(normQuery, plusQuery, minusQuery);
    const parsedResponse = JSON.parse(response);
    this.outPutResults.emit(parsedResponse);
  }
}
