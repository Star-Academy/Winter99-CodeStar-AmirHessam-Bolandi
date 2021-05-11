import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';

@Component({
  selector: 'app-results',
  templateUrl: './results.component.html',
  styleUrls: ['./results.component.scss']
})
export class ResultsComponent implements OnInit {
  @Input()
  public resultValue: string;
  // @Output()
  // public outPutResultClick: EventEmitter<string[]> = new EventEmitter<string[]>();

  constructor() {
  }

  ngOnInit(): void {
  }

  public fileHandler(event: MouseEvent): void {

    // todo : open file in root and ...
  }
}
