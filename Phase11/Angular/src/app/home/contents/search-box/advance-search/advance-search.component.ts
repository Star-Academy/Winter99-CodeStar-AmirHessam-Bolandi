import {Component, EventEmitter, OnInit, Output} from '@angular/core';

@Component({
  selector: 'app-advance-search',
  templateUrl: './advance-search.component.html',
  styleUrls: ['./advance-search.component.scss']
})
export class AdvanceSearchComponent implements OnInit {
  public plusValue: string;
  public minusValue: string;
  @Output()
  public outPutPlusValue: EventEmitter<string> = new EventEmitter<string>();
  @Output()
  public outPutMinusValue: EventEmitter<string> = new EventEmitter<string>();
  constructor() {
  }

  ngOnInit(): void {
  }

  public passValue(): void {
    this.outPutPlusValue.emit(this.plusValue);
    this.outPutMinusValue.emit(this.minusValue);
  }

}
