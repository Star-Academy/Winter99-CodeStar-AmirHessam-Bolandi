import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';

@Component({
  selector: 'app-normal-search',
  templateUrl: './normal-search.component.html',
  styleUrls: ['./normal-search.component.scss']
})
export class NormalSearchComponent implements OnInit {
  public normalInp: string;
  @Input()
  public buttonLabel: string;
  @Output()
  public outPutNormalValue: EventEmitter<string> = new EventEmitter<string>();
  @Output()
  public outPutButtonClick: EventEmitter<string> = new EventEmitter<string>();


  constructor() {
    this.buttonLabel = 'جست و جوی پیشرفته';

  }

  ngOnInit(): void {
  }

  public passValue(): void {
    this.outPutNormalValue.emit(this.normalInp);
  }

  public buttonHandler(): void {
    this.outPutButtonClick.emit();
  }
}
