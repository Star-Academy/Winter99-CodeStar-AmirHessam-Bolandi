import {Component, EventEmitter, OnInit, Output} from '@angular/core';

@Component({
  selector: 'app-normal-search',
  templateUrl: './normal-search.component.html',
  styleUrls: ['./normal-search.component.scss']
})
export class NormalSearchComponent implements OnInit {
  public normalInp: string;
  public buttonLabel: string;
  @Output()
  public outPutNoramlValue: EventEmitter<string> = new EventEmitter<string>();
  @Output()
  public outPutButtonClick: EventEmitter<string> = new EventEmitter<string>();


  constructor() {
    this.buttonLabel = 'جست و جوی پیشرفته';
  }

  ngOnInit(): void {
  }

  public passValue(): void {
    this.outPutNoramlValue.emit(this.normalInp);
  }

  public buttonHandler(): void {
    this.outPutButtonClick.emit();
  }
}
